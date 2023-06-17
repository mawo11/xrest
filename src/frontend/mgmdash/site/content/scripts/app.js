var app = new Vue({
    el: '#app',
    data: {
        items: []
    },
    created: function () {
        this.items = menuItems;
    },
    methods: {
        callApp: function(item) {
            window.open(item.link)
        }
    }
});