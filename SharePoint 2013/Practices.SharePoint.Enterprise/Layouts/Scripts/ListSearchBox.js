ExecuteOrDelayUntilEventNotified(function () {
    Microsoft.SharePoint.Portal.ListSearchBox.prototype.$g_3 = function (a, b) {        
        this.$O_3 = Microsoft.SharePoint.Portal.ListSearchBox.$y(this.$S_3, $p0);
        this.$19_3(this.$O_3 === 2);
        this.set_$I_3($p1);
        if (this.$O_3 !== 3) {
            this.$r_3();
            this.$0_3.disabled = true;
            this.$7_3 = 2;
            this.$3_3.focus();
            this.$4_3 = true;
            window.setTimeout(this.$$d_$1W_3, 300);
            this.$1_3.onDataRefreshCompleted = this.$$d_onDataRefreshCompleted;
            var queryString = "<Query><Where><Contains><FieldRef Name='Title' /><Value Type='Text'>" + b + "</Value></Contains></Where></Query>";
            this.$1_3.queryString = queryString;
            this.$1_3.seatchTerm = "";
            inplview.RefreshInplViewUrlByContext(this.$1_3);
            inplview.HandleRefreshViewByContext(this.$1_3);
            this.updateVisuals();
        }
        else {
            this.$1Y_3();
        }
    }
}, "sp.scriptloaded-SP.UI.ListSearchBox.js");