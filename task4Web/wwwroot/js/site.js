
// Obtener todos los checkboxes
const checkboxes = document.querySelectorAll('.seleccionar');
function unselectSelectAll(masterCheckbox) {
    if (masterCheckbox.checked) {
        checkboxes.forEach(cb =>
        {
            cb.checked = masterCheckbox.checked;
        });
    }
    else{
        checkboxes.forEach(cb =>
        {
            cb.checked = masterCheckbox.checked;
        });
     }
 }

function getUsersSelected()
{
    let users = [];
    checkboxes.forEach(function (checkbox)
    {
        if (checkbox.checked)
        {
            const fila = checkbox.closest('tr');
            const id = fila.children[1].textContent;
            users.push(id)
        }
    });
    return users.toString();
}

function blockUsers(listUsers) {
    listUsers = getUsersSelected();
    fetch('https://www.task4api.somee.com/api/Users/BlockUsers',
    {
        method: 'POST',
        headers:
        {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(listUsers)
        }).then(response => response.ok && location.reload());   
}

function unblockUsers(listUsers) {
    listUsers = getUsersSelected();
    fetch('https://www.task4api.somee.com/api/Users/UnblockUsers', {
    method: 'POST',
headers: {
    'Content-Type': 'application/json'
            },
body: JSON.stringify(listUsers)
        }).then(response => response.ok && location.reload());
    }

function deleteUsers(listUsers) {
    listUsers = getUsersSelected();
    fetch('https://www.task4api.somee.com/api/Users/DeleteUsers?ids=' + listUsers, {
    method: 'POST',
headers: {
    'Content-Type': 'application/json'
            }
        }).then(response => response.ok && location.reload());
           
    }
