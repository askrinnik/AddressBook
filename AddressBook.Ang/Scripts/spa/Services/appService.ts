class AppService {
  clearSearchResult() {
    $("#SearchResults").empty();
  }

  logoff() {
    (document.getElementById("logoutForm") as HTMLFormElement).submit();
  }

  addDatePicker(controlName: string) {
    $(`#${controlName}`).datepicker();
  }
}

let appService = new AppService();

// the simplest but not OOP way:
//let appService = {
//  clearSearchResult: () => {
//     $("#SearchResults").empty();
//  }
//};
