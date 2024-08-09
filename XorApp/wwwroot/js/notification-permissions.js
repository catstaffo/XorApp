const { Notification } = require('electron');

function requestNotificationPermission() {
  Notification.requestPermission().then((permission) => {
    if (permission === 'granted') {
      console.log('Notification permission granted.');
    } else if (permission === 'denied') {
      console.log('Notification permission denied.');
    } else {
      console.log('Notification permission default.');
    }
  });
}

module.exports = {
  requestNotificationPermission,
};

