
// Get the current date and time
const now = new Date();

// Initialize Tempus Dominus for startDatePicker
const startDatePicker = new tempusDominus.TempusDominus(document.getElementById('startDatePicker'), {
    restrictions: {
        minDate: now // Prevent selecting dates before the current date
    },
    display: {
        icons: {

            date: 'fa-solid fa-calendar',
            up: 'fa-solid fa-arrow-up',
            down: 'fa-solid fa-arrow-down',
            previous: 'fa-solid fa-chevron-left',
            next: 'fa-solid fa-chevron-right',
            today: 'fa-solid fa-calendar-check',
            clear: 'fa-solid fa-trash',
            close: 'fa-solid fa-xmark'
        },
        viewMode: 'calendar',
        buttons: {
            today: true,
            clear: true,
            close: true
        },
        components: {
            calendar: true,
            date: true,
            month: true,
            year: true,
            decades: true,
            clock: false // Only show the calendar (no time picker)
        }
    },
    localization: {
        format: 'dd-MM-yyyy',
    }
});

// Initialize Tempus Dominus for endDatePicker
const endDatePicker = new tempusDominus.TempusDominus(document.getElementById('endDatePicker'), {
    restrictions: {
        minDate: now // Prevent selecting dates before the current date
    },
    display: {
        icons: {
            time: 'fa-solid fa-clock',
            date: 'fa-solid fa-calendar',
            up: 'fa-solid fa-arrow-up',
            down: 'fa-solid fa-arrow-down',
            previous: 'fa-solid fa-chevron-left',
            next: 'fa-solid fa-chevron-right',
            today: 'fa-solid fa-calendar-check',
            clear: 'fa-solid fa-trash',
            close: 'fa-solid fa-xmark'
        },
        viewMode: 'calendar',
        buttons: {
            today: true,
            clear: true,
            close: true
        },
        components: {
            calendar: true,
            date: true,
            month: true,
            year: true,
            decades: true,
            clock: false // Only show the calendar (no time picker)
        },

    },
    localization: {
        format: 'dd-MM-yyyy',
    },
    
});

endDatePicker.updateOptions({
    useCurrent: false 
});
