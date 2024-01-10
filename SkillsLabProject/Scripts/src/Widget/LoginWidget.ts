/// <amd-dependency path="dojo/_base/declare" name="declare"/>
/// <amd-dependency path="dijit/_Widget" name="_Widget"/>
/// <amd-dependency path="dijit/_TemplatedMixin" name="_TemplatedMixin"/>
/// <amd-dependency path="dijit/_WidgetsInTemplateMixin" name="_WidgetsInTemplateMixin"/>
/// <amd-dependency path="dojo/text!/StaticViews/LoginWidget.html" name="template"/>

declare const declare;
declare const _Widget;
declare const _TemplatedMixin;
declare const _WidgetsInTemplateMixin;
declare const template;

class LoginWidget {
    own: (value: any) => void;
    set: (key: string, value: any) => void;
    _set: (key: string, value: any) => void;
    inherited: (value: any) => void;

    private templateString: any = template;

    constructor() {
        console.log('constructor');
    }

    postCreate() {
        this.inherited(arguments);
        console.log('Create Post');
    }

    startup() {
        this.inherited(arguments);
    }
}

export = declare("Widget.LoginWidget", [_Widget, _TemplatedMixin, _WidgetsInTemplateMixin], {
    templateString: template,

    constructor: function () {
        console.log('constructor');
    },

    postCreate: function () {
        this.inherited(arguments);
        console.log('Create Post');
    },

    startup: function () {
        this.inherited(arguments);
    }
});
