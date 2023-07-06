mergeInto(LibraryManager.library, {

	FindGroup: function () {
		var str = "A string passed from JavaScript to C#";
    var bufferSize = lengthBytesUTF8(str) + 1; // calculate the size of null-terminated UTF-8 string
    var buffer = _malloc(bufferSize); // allocate string buffer on the heap
    stringToUTF8(str, buffer, bufferSize); // fill the buffer with the string UTF-8 value
    return buffer; // return the pointer of the allocated string to C#
	},  

});