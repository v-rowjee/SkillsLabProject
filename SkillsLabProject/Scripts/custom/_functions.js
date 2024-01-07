class App {
    static showSnackbar(message) {
        Snackbar.show({
            text: message,
            actionTextColor: "#CFE2FF"
        });
    }

    static performAjaxRequest(requestParams) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: requestParams.method,
                url: requestParams.url,
                data: requestParams.data,
                dataType: "json",
                success: (response) => {
                    if (response.IsSuccess) {
                        App.showSnackbar(response.Message);
                        resolve(response);
                    } else {
                        reject(response);
                    }
                },
                error: () => {
                    reject({ IsSuccess: false, Message: "An error occurred while making the request." });
                }
            });
        })
            .then((response) => {
                App.showSnackbar(response.Message);

                if (response.RedirectUrl) {
                    setTimeout(() => {
                        window.location.replace(response.RedirectUrl);
                    }, 1000);
                }
                return response;
            })
            .catch((error) => {
                App.showSnackbar(error.Message);
                throw error;
            });
    }

    static getDefaultTheme() {
    //return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    return 'light';
}

    static updateTheme(theme) {
        localStorage.setItem("theme", theme);

        $("html").attr("data-bs-theme", theme)

        var classBefore = theme === "dark" ? "bg-light btn-light" : "bg-dark-subtle btn-dark";
        var classAfter = theme === "dark" ? "bg-dark-subtle btn-dark" : "bg-light btn-light";
        $(".bg-light, .btn-light").removeClass(classBefore).addClass(classAfter);

        var darkModeToggleText = theme === "dark" ? "Light Mode" : "Dark Mode";
        $("#darkModeToggle").text(darkModeToggleText);
    }

}
export default App;