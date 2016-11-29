var appService = {
  clearSearchResult: function() {
    $("#SearchResults").empty();
  },

  logoff: function() {
    document.getElementById('logoutForm').submit();
  },

  addDatePicker: function(controlName) {
    $("#" + controlName).datepicker();
  }
};
