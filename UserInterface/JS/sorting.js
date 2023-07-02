$("#filterCheck").on("change", function () {
    if ($(this).is(':checked')) {
        filterAvailable(true);
    } else {
        filterAvailable(false);
    }
});

function sortName(desc) {
    filtered.sort((a, b) => {
        const nameA = a.Name.toUpperCase();
        const nameB = b.Name.toUpperCase();
        if (desc) {
            if (nameA < nameB) return 1;
            if (nameA > nameB) return -1;
            return 0;
        } else {
            if (nameA < nameB) return -1;
            if (nameA > nameB) return 1;
            return 0;
        }

    });
    populateItems(filtered);
}
function sortUserName(desc) {
    filteredUsers.sort((a, b) => {
        const nameA = a.FullName.toUpperCase();
        const nameB = b.FullName.toUpperCase();

        if (desc) {
            if (nameA < nameB) return 1;
            if (nameA > nameB) return -1;
            return 0;
        } else {
            if (nameA < nameB) return -1;
            if (nameA > nameB) return 1;
            return 0;
        }
    });
    populateUserTable(filteredUsers);
}
function sortPrice(desc) {
    filtered.sort((a, b) => {
        const priceA = a.Price;
        const priceB = b.Price;

        if (desc) {
            return priceB - priceA; // Descending order
        } else {
            return priceA - priceB; // Ascending order
        }
    });
    populateItems(filtered);
}
function sortRole(desc) {
    filteredUsers.sort((a, b) => {
        const roleA = a.Role;
        const roleB = b.Role;

        if (desc) {
            return roleB - roleA; // Descending order
        } else {
            return roleA - roleB; // Ascending order
        }
    });
    populateUserTable(filteredUsers);
}
function sortDate(desc) {
    filtered.sort((a, b) => {
        const dateA = new Date(a.PublishDate).getTime();
        const dateB = new Date(b.PublishDate).getTime();
        console.log(dateA +" | "+ dateB);

        if (desc) {
            return dateB - dateA; // Descending order
        } else {
            return dateA - dateB; // Ascending order
        }
    });
    populateItems(filtered);
}
function sortBirthDate(desc) {
    filteredUsers.sort((a, b) => {
        const dateA = new Date(a.DateOfBirth);
        const dateB = new Date(b.DateOfBirth);

        if (desc) {
            return dateB - dateA; // Descending order
        } else {
            return dateA - dateB; // Ascending order
        }
    });
    populateUserTable(filteredUsers);
}

function filterAvailable(yes_no) {
    filtered = products.filter(x => x.isAvailable == yes_no);
    populateItems(filtered);
}
