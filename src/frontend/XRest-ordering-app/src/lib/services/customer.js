import { writable, get } from 'svelte/store'
import { addMinutes, getObjectFromSessionStorage, getSelectedLang} from '../helpers'
import { push } from '../router'

let storedAuthData = getObjectFromSessionStorage('authData')

export const authData = writable(storedAuthData || {
    token: null,
    refreshToken: null,
    expireIn: -1,
    isLogged: false
})

authData.subscribe(value => {
    sessionStorage.setItem('authData', JSON.stringify(value))
})

let callbackToFireWhenUnauthorized = null
export function setCallbackToFireWhenUnauthorized( callback ){
    callbackToFireWhenUnauthorized = callback
}

export function logon(request, callback) {
    const url = `${apiUrl}/identity-api/customer/logon`
    fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    }).catch(()=>{
        push('/error') 
    })
        .then(response => response.json())
        .then(response => {
            if (response.status == 'ok') {
                authData.set({
                    token: response.token.access_token,
                    refreshToken: response.token.refresh_token,
                    expireIn: response.token.expires_in,
                    isLogged: true,
                    user: response.firstname || response.email,
                    unsubscribeHandle: null,
                    expierInDate: addMinutes(new Date(), response.token.expires_in ),
                    firstname: response.firstname,
                    email: response.email,
                    phone: response.phone
                })
                setRefrestTokenCallback()
            }

            callback(response.status);
        })
        
}

export function setRefrestTokenCallback() {
    if (get(authData).unsubscribeHandle != null) {
        clearTimeout(get(authData).unsubscribeHandle)
    }

    if (!get(authData).isLogged) {
        return
    }

    let timeout =(new Date(get(authData).expierInDate).getTime() - (new Date()).getTime())  - 10 * 1000
    let unsubscribeHandle = setTimeout( () => {
        refreshToken(get(authData).refreshToken)
    },timeout )

    authData.update( s => {
        s.unsubscribeHandle = unsubscribeHandle
        return s;
    })
}

function refreshToken(token) {
    const url = `${apiUrl}/identity-api/customer/refresh-token`
    return fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Authorization': 'bearer ' + token
        }
    }).catch( () => {
        authData.set({
            token: null,
            refreshToken: null,
            expireIn: -1,
            isLogged: false,
            user: null,
            firstname: null,
            email: null,
            phone: null
        })
    })
    .then(response => response.json())
    .then(response => {
        authData.update( s => {
            s.token = response.access_token
            s.refreshToken = response.refresh_token
            s.expireIn = response.expires_in
            s.expierInDate = addMinutes(new Date(), response.expires_in )
            return s
        })

        setRefrestTokenCallback()
        return response
    })
}

export function logoff() {
    try {
        if (get(authData).unsubscribeHandle != null) {
            clearTimeout(get(authData).unsubscribeHandle)
        }
        authData.set({
            token: null,
            refreshToken: null,
            expireIn: -1,
            isLogged: false,
            user: null,
            firstname: null,
            email: null,
            phone: null
        })
    }catch(e) {
    }
}

export async function newAccount(account) {
    const url = `${apiUrl}/identity-api/customer/new-account`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(account)
    }).catch(()=>{
        push('/error')
    })
    return await response.json()
}

export async function activateAccount(token) {
    const url = `${apiUrl}/identity-api/customer/${token}/activate`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json'
        }
    }).catch(()=>{
        push('/error')
    })

    return await response.json()
}

export async function sendResetPassword(request) {
    const url = `${apiUrl}/identity-api/customer/send-reset-password`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    }).catch(()=>{
        push('/error')
    })

    return await response.json()
}

export async function resetPassword( request) {
    const url = `${apiUrl}/identity-api/customer/reset-password`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    }).catch(()=>{
        push('/error')
    })

    return await response.json()
}

export async function myProfileGetDetails() {
    const url = `${apiUrl}/identity-api/customer/details`
    const request = new Request(url, {
        method: 'GET',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Authorization': 'bearer ' + get(authData).token
        }
    })

    return await secureCall(request)
}

export async function myProfileUpdateDetails(data) {
    const url = `${apiUrl}/identity-api/customer/update-details`
    const request = new Request(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'bearer ' + get(authData).token
        },
        body: JSON.stringify(data)
    })

    return await secureCall(request)
}

export async function myProfileChangeMyPassword(data) {
    const url = `${apiUrl}/identity-api/customer/change-my-password`
    const request = new Request(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'bearer ' + get(authData).token
        },
        body: JSON.stringify(data)
    })

    return await secureCall(request)
}


export async function myProfileRemoveAccount() {
    const url = `${apiUrl}/identity-api/customer/remove-account`
    const request = new Request(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Authorization': 'bearer ' + get(authData).token
        }
    })

    return await secureCall(request)
}

export async function myProfileGetPointsHistory() {
    const url = `${apiUrl}/identity-api/customer/my-points`
 
    const request = new Request(url, {
        method: 'GET', 
        cache: "no-store",
       // mode: 'no-cors',
        headers: {
            'Authorization': 'bearer ' + get(authData).token,
   
        }
    })
    
    return await secureCall(request)
}

async function secureCall( request ) {
    const response = await fetch(request)

    if (response.status === 401) {
        const result = refreshToken(get(authData).refreshToken)
        if (result.status == 200) {
            const secondTry = await fetch(request)
            if (secondTry.status === 200) {
                return response.json()
            }
        } 

        await logoff()
        try{
            if (callbackToFireWhenUnauthorized != null){
                callbackToFireWhenUnauthorized()
            }
        }catch(e){}
        return Promise.reject()
    } else if(response.status === 200){
        return response.json()
    }else {
        push('/error')
    }
}

export async function myProfileIncraseMarketingAgreementsTries() {
    const url = `${apiUrl}/identity-api/customer/my-points`
    const request = new Request(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Authorization': 'bearer ' + get(authData).token
        }
    })

    return await secureCall(request)
}

export async function myProfileGetMarketingAgreements() {
    const url = `${apiUrl}/identity-api/customer/marketing-agreements?lang=${getSelectedLang()}`
    const request = new Request(url, {
        method: 'GET',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Authorization': 'bearer ' + get(authData).token
        }
    })

    return await secureCall(request)
}

export async function myProfileSaveMarketingAgreements(data) {
    const url = `${apiUrl}/identity-api/customer/save-marketing-agreements`
    const request = new Request(url, {
        method: 'GET',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Authorization': 'bearer ' + get(authData).token
        },
        body: JSON.stringify(data)
    })

    return await secureCall(request)
}

export async function myProfileGetFavoriteAddressList() {
    const url = `${apiUrl}/identity-api/favoriteaddress/list`
    const request = new Request(url, {
        method: 'GET',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Authorization': 'bearer ' + get(authData).token
        }
    })

    return await secureCall(request)
}

export async function myProfileRemoveFavoriteAddress(id) {
    const url = `${apiUrl}/identity-api/favoriteaddress/${encodeURIComponent(id)}/remove`
    const request = new Request(url, {
        method: 'DELETE',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Authorization': 'bearer ' + get(authData).token
        }
    })

    return await secureCall(request)
}

export async function myProfileSaveFavoriteAddress(data) {
    const url = `${apiUrl}/identity-api/favoriteaddress/save`
    const request = new Request(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Authorization': 'bearer ' + get(authData).token,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })

    return await secureCall(request)
}

export async function myProfileGetMyOrders() {
    const url = `${apiUrl}/identity-api/customer/my-orders`
    const request = new Request(url, {
        method: 'GET',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Authorization': 'bearer ' + get(authData).token
        }
    })

    return await secureCall(request)
}