var Tips2 = new Tips($$('.Tips2'), {
    initialize: function () {
        this.fx = new Fx.Style(this.toolTip, 'opacity', { duration: 500, wait: false }).set(0);
    },
    onShow: function (toolTip) {
        this.fx.start(1);
    },
    onHide: function (toolTip) {
        this.fx.start(0);
    }
});
