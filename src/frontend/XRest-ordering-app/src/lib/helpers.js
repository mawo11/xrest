import { toast } from '@zerodevx/svelte-toast'

export function isEmpty(value) {
    return (value == null || value.length === 0);
}

export function isValidEmail(value) {
    if(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(value)) {
        return true;
    }
    return false;
}

export function isValidPassword(pass1, pass2) {
     return !(isEmpty(pass1) || isEmpty(pass2) || pass1 !=pass2)
}

export function addMinutes(date, minutes) {
    return new Date(date.getTime() + minutes * 60000);
}

export function pushNormalToast(text) {
    toast.push(text, {
        theme: {
            '--toastBackground': '#48BB78',
            '--toastBarBackground': '#2F855A'
        }
    })
}

export function pushWarnToast(text) {
    toast.push(text, {
        theme: {
            '--toastBackground': '#F56565',
            '--toastBarBackground': '#C53030'
        }
    })
}

export function getObjectFromSessionStorage(keyname) {
    let data = sessionStorage.getItem(keyname)
    if (data != null) {
        try{
            data = JSON.parse(data)
        }catch(e){
            data = null
            sessionStorage.removeItem(keyname)
        }
    }

    return data
}

export function getSelectedLang() {
    const selectedLang = sessionStorage.getItem('lang')

    return selectedLang == null ? "pl" : selectedLang
}

export function saveSelectedLang(lang) {
    sessionStorage.setItem('lang', lang)
}