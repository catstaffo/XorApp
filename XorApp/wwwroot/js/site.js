document.addEventListener('DOMContentLoaded', () => {
  const { ipcRenderer } = require('electron');

  // TODO
  ipcRenderer.on('window-resized', (event, size) => {
    console.log('Window resized:', size);

  });
});

// TODO
window.addEventListener('resize', () => {
});

document.getElementById('binaryFile').addEventListener('change', function(e) {
  var file = e.target.files[0];
  if (file) {
    document.getElementById('filePath').value = file.name;

    var fileSizeBytes = file.size;
    var fileSizeKB = (fileSizeBytes / 1024).toFixed(2);
    var fileSizeString = fileSizeBytes + " bytes (" + fileSizeKB + " KB)";

    document.getElementById('fileSize').value = fileSizeString;
  }
});
