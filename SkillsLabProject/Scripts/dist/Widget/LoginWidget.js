/// <amd-dependency path="dojo/_base/declare" name="declare"/>
/// <amd-dependency path="dijit/_Widget" name="_Widget"/>
/// <amd-dependency path="dijit/_TemplatedMixin" name="_TemplatedMixin"/>
/// <amd-dependency path="dijit/_WidgetsInTemplateMixin" name="_WidgetsInTemplateMixin"/>
/// <amd-dependency path="dojo/text!/StaticViews/LoginWidget.html" name="template"/>
define(["require", "exports", "dojo/_base/declare", "dijit/_Widget", "dijit/_TemplatedMixin", "dijit/_WidgetsInTemplateMixin", "dojo/text!/StaticViews/LoginWidget.html"], function (require, exports, declare, _Widget, _TemplatedMixin, _WidgetsInTemplateMixin, template) {
    "use strict";
    var LoginWidget = /** @class */ (function () {
        function LoginWidget() {
            this.templateString = template;
            console.log('constructor');
        }
        LoginWidget.prototype.postCreate = function () {
            this.inherited(arguments);
            console.log('Create Post');
        };
        return LoginWidget;
    }());
    return declare("widget/LoginWidget", [_Widget, _TemplatedMixin, _WidgetsInTemplateMixin], LoginWidget.prototype);
});
