/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
	// Define changes to default configuration here. For example:
	 config.language = 'vi';
    // config.uiColor = '#AADC6E';
     config.height = '420px';
    
    config.enterMode = CKEDITOR.ENTER_BR;
    //config.toolbar = 'Basic';
    //config.toolbar = [
    //['Source', '-', 'NewPage', 'Preview', '-', 'Templates'],
    //['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'],
    //'/',
    //['Bold', 'Italic']
    //];

    config.filebrowserBrowseUrl = '/Content/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/Content/ckfinder/ckfinder.html?type=Images';
    config.filebrowserFlashBrowseUrl = '/Content/ckfinder/ckfinder.html?type=Flash';
    config.filebrowserUploadUrl = '/Content/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Content/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '/Content/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
    config.filebrowserWindowWidth = '1000';
    config.filebrowserWindowHeight = '700';
};
//cau hinh don gian 
//CKEDITOR.config.toolbar = [
//['Bold', 'Italic', 'Underline', 'Superscript', '-', 'NumberedList', 'BulletedList', 'Outdent', 'Indent', 'Blockquote', '-', 'Format', ],
//['JustifyLeft', 'JustifyCenter', 'JustifyRight', ],
//['Image', '-', 'Link', 'Unlink', '-', 'Source']
//];
CKEDITOR.config.toolbar = [['Source', '-', 'NewPage', 'Preview', '-', 'Templates'], ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo', 'Find', 'Replace', '-', 'Outdent', 'Indent', '-', 'Print'],
    '/',
    ['Bold', 'Italic'], ['Styles', 'Format', 'Font', 'FontSize'], ['Image', 'Table', '-', 'Link', 'Flash', 'Smiley', 'TextColor', 'BGColor'],
    ['NumberedList', 'BulletedList', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock']
    ];