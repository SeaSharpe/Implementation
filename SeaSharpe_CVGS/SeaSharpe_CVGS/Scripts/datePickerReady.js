/*
* datePickerReady.js
* Revision History:
*   18/11/2015 Peter Thomson: Created
*/

//applies the datepicker ui to the datefield class
$(function ()
{
    //Set datefield class elements to datepicker 
    $(".datefield").datepicker({ dateFormat: 'dd-MM-yy' });

    //Jquery date validation format
    $.validator.methods.date = function (value, element)
    {
        return this.optional(element) || Date.parseExact(value, "dd-MM-yyyy");
    };
});