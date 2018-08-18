var Sequelize = require('sequelize');
var db = {};

var sequelize = new Sequelize('null', 'null', 'null', {
  dialect: 'sqlite',
  storage: 'test.sqlite'
});

db.User = sequelize.define('user', {
  username: { type: Sequelize.STRING },
  password: { type: Sequelize.STRING },
  status: { type: Sequelize.STRING } //New/Verified
});

db.SellOrder = sequelize.define('order', {
  status: { type: Sequelize.STRING }, //Pending/Resolved
  itemA: { type: Sequelize.STRING },
  itemB: { type: Sequelize.STRING },
  price: { type: Sequelize.INTEGER }
});
db.User.hasMany(db.SellOrder);
db.SellOrder.belongsTo(db.User);

db.BuyOrder = sequelize.define('order', {
  status: { type: Sequelize.STRING }, //Pending/Resolved
  itemA: { type: Sequelize.STRING },
  itemB: { type: Sequelize.STRING },
  price: { type: Sequelize.INTEGER }
});
db.User.hasMany(db.BuyOrder);
db.BuyOrder.belongsTo(db.User);

db.Transaction = sequelize.define('transaction', {
  userAId: { type: Sequelize.STRING },
  userBId: { type: Sequelize.STRING },
  itemA: { type: Sequelize.STRING },
  itemB: { type: Sequelize.STRING },
  price: { type: Sequelize.INTEGER }
});

sequelize.sync({ force: true });

module.exports = db;
