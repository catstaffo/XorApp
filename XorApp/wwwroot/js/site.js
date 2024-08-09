// TODO
document.addEventListener('DOMContentLoaded', () => {
  const { ipcRenderer } = require('electron');
  ipcRenderer.on('window-resized', (event, size) => {
    console.log('Window resized:', size);

  });
});

// TODO
$(window).on('resize', function() {
});
