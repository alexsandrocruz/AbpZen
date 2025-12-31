//Should remove when https://github.com/Megabit/Blazorise/issues/4287 is fixed

const rawError = console.error;
console.error = function (...args) {
    if (
        args[0].includes("Exceptions were encountered while disposing components") ||
        (args[0].includes("JS object instance with ID") && args[0].includes("does not exist (has it been disposed?)")))
    {

        document.querySelector("#blazor-error-ui").remove();
        location.reload()
    }
    else
    {
        rawError.apply(this, args);
    }
}
