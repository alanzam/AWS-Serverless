var express = require('express');
var bodyParser = require('body-parser');
var jwtSign = require('jsonwebtoken');
var jwt = require('express-jwt');
var fileUpload = require('express-fileupload');
var db = require('./db');
var Sequelize = require('sequelize');
var Op = Sequelize.Op;
var app = express();
var uuidv1 = require('uuid/v1');
var path = require('path');

var secretKey = 'Key123456';

// To use JSON in bodies
app.use(bodyParser.json());
app.use(jwt({ secret: secretKey}).unless({path: ['/login', '/register']}));
app.use(fileUpload());


//Accounts
app.post('/register', (req, res) => {
   console.log(req.body);
   db.User.create({
     username: req.body.username,
     password: req.body.password
   }).then(user => res.send(user));
});

app.post('/login', (req, res) => {
  console.log(req.body);
  db.User.find({ where: { username: req.body.username, password: req.body.password} }).then(user => {
    if (!user)
      return res.status(404).send('Invalid Login');
    res.send(jwtSign.sign(user.id, secretKey));
  });
});

//Image
app.post('/uploadImage', function(req, res) {
  console.log(req.user);
  if (!req.files)
    return res.status(400).send('No files were uploaded.');
  var sampleFile = req.files.image;
  sampleFile.mv('uploads/' + req.user + '.jpg', function(err) {
    if (err)
      return res.status(500).send(err);
    res.send('File uploaded!');
  });
});

app.get('/getImage', function(req, res) {
  res.sendFile('./uploads/' + req.user + '.jpg');
})


//Buy Orders
app.post('/orders/buy', (req, res) => {
  console.log(req.body);
  db.SellOrder.find({where: { itemB: req.body.itemA, price: req.body.price, status: 'Pending'} })
  .then(sellOrder =>
    {
      if (!sellOrder)
        return db.BuyOrder.create({
          userId: req.user,
          itemA: req.body.itemA,
          itemB: req.body.itemB,
          status: 'Pending',
          price: req.body.price,
        });
      console.log(sellOrder);
      sellOrder.updateAttributes({status: 'Resolved'});
      return db.Transaction.create({
        userAId: req.user,
        userBId: sellOrder.userId,
        itemA: sellOrder.itemB,
        itemB: sellOrder.itemA,
        price: sellOrder.price
      })
      .then(trans => db.BuyOrder.create({
        userId: req.user,
        itemA: trans.itemB,
        itemB: trans.itemA,
        status: 'Resolved',
        price: trans.price,
      }));
    })
  .then(order => res.status(200).send(order));
});

app.get('/orders/buy', (req, res) => {
  console.log(req.user);
  db.BuyOrder.findAll({ where: { userId: req.user} }).then(orders => {
    res.send(orders);
  });
});

//Buy Orders
app.post('/orders/sell', (req, res) => {
  console.log(req.body);
  db.BuyOrder.find({where: { itemB: req.body.itemA, price: req.body.price, status: 'Pending'} })
  .then(buyOrder =>
    {
      if (!buyOrder)
        return db.SellOrder.create({
          userId: req.user,
          itemA: req.body.itemA,
          itemB: req.body.itemB,
          status: 'Pending',
          price: req.body.price,
        });
      console.log(buyOrder);
      buyOrder.updateAttributes({status: 'Resolved'});
      return db.Transaction.create({
        userAId: req.user,
        userBId: buyOrder.userId,
        itemA: buyOrder.itemB,
        itemB: buyOrder.itemA,
        price: buyOrder.price
      })
      .then(trans => db.SellOrder.create({
        userId: req.user,
        itemA: trans.itemB,
        itemB: trans.itemA,
        status: 'Resolved',
        price: trans.price,
      }));
    })
  .then(order => res.status(200).send(order));
});

app.get('/orders/sell', (req, res) => {
  console.log(req.user);
  db.SellOrder.findAll({ where: { userId: req.user} }).then(orders => {
    res.send(orders);
  });
});


//Transactions
app.get('/transactions', (req, res) => {
  console.log(req.user);
  db.Transaction.findAll({ where: {[Op.or]: [{ userAId: req.user}, { userBId: req.user}] } }).then(trans => {
    res.send(trans);
  });
});

app.listen(3000, () => console.log('Running Server'));
