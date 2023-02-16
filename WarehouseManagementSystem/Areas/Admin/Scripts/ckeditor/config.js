/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */
CKEDITOR.timestamp = 'AAAAB'; 
CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example
    config.toolbarGroups = [
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
        { name: 'clipboard', groups: ['clipboard', 'undo'] },
        { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
        { name: 'forms', groups: ['forms'] },
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
        { name: 'links', groups: ['links'] },
        { name: 'styles', groups: ['styles'] },
        { name: 'insert', groups: ['insert'] },
        { name: 'colors', groups: ['colors'] },
        { name: 'tools', groups: ['tools'] },
        { name: 'others', groups: ['others'] },
        { name: 'document', groups: ['mode', 'document', 'doctools'] },
        { name: 'about', groups: ['about'] }
    ];

    config.removeButtons = 'Form,Checkbox,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,Replace,Scayt,Save,NewPage,Preview,Print,Cut,Copy,PasteFromWord,PasteText,Paste,BidiLtr,BidiRtl,Language,Anchor,Image,Embed,Flash,Smiley,SpecialChar,BGColor,About';

    config.language = 'tr';
    config.embed_provider = '//ckeditor.iframe.ly/api/oembed?url={url}&callback={callback}';
    config.extraPlugins = 'embed,autoembed,base64image,tableresizerowandcolumn,imageresizerowandcolumn';
    //config.allowedContent = true;

};
