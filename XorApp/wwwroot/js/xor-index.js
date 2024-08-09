$('#binaryFile').on('change', function(e){
  var file = e.target.files[0];
  if (!file) return;
  $('#filePath').val(file.name);
  var fileSizeBytes = file.size;
  var fileSizeKB = (fileSizeBytes / 1024).toFixed(2);
  $('#fileSize').val(`${fileSizeBytes} bytes (${fileSizeKB} KB)`);
});

$('#js-generateKey').on('click', function() {
  var bitLength = $('#xorSize').val();
  console.log(bitLength);
  generateKeyValues(bitLength);
})

function generateKeyValues(bitLength) {
  console.log(bitLength);
  console.log(typeof(bitLength));
  $.ajax({
    url: '/Xor/GenerateKey/',
    type: 'POST',
    data: {
      bitLength : parseInt(bitLength)
    },
    success: function(data) {
      var keyObj = JSON.parse(data);
      setKeyValues(keyObj);
    },
    error: function(error) {
      console.error(error);
    }
  })

  function setKeyValues(keyObj) {
    $('input#currentKey').val(keyObj.CurrentKey);
    $('input#currentValueHex').val(keyObj.CurrentValueHex);
    $('input#currentValueBinary').val(keyObj.CurrentValueBinary);
    $('input#currentValueInt').val(keyObj.CurrentValueInt);
  }
}
