import { writable, get } from 'svelte/store'
import { authData } from './customer'
import { isEmpty, getObjectFromSessionStorage, getSelectedLang } from '../helpers'
import { push } from '../router'

let storedBasketData = getObjectFromSessionStorage('basketData')

export const basketData = writable(storedBasketData || {
    basketKey: null,
    zoneId: 0,
    deliveryMode: 0,
    restaurantId: 0,
    discountCode: null,
    discountLoaded: false
})

basketData.subscribe(value => {
    sessionStorage.setItem('basketData', JSON.stringify(value))
})

export function basketRefreshFromSession() {
    let storedBasketData = getObjectFromSessionStorage('basketData')
    basketData.set(storedBasketData)
}

export function basketInit(restaurantId, zoneId, deliveryMode) {
    const url = `${apiUrl}/orders-api/basket/init/${encodeURIComponent(restaurantId)}/${encodeURIComponent(zoneId)}/${encodeURIComponent(deliveryMode)}?basketKey=${get(basketData).basketKey}`

    return fetch(url, {
        method: 'POST',
        cache: "no-store"
    })
        .then(response => response.json())
        .then((basketInitResponse) => {
            basketData.update(state => {
                state.basketKey = basketInitResponse.key
                state.zoneId = zoneId
                state.deliveryMode = deliveryMode
                state.restaurantId = restaurantId
                state.discountCode = null
                state.discountLoaded = false
                return state
            })
        })
}

export function basketClear() {
    basketData.update(state => {
        state.basketKey = null
        state.zoneId = 0
        state.deliveryMode = 0
        state.restaurantId = 0
        state.discountCode = null
        state.discountLoaded = false
        return state
    })

    basketRemoveAll()
        .then(() => { })
}

export async function basketChangeDelivery(delivery) {
    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/change-delivery-type/${encodeURIComponent(delivery)}`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store"
    }).catch(() => {
        push('/error')
    })

    return await response.json()
}

export async function basketRemoveAll() {
    let basketKey = get(basketData).basketKey
    if (isEmpty(basketKey)) {
        return
    }

    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(basketKey)}/remove-all`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store"

    }).catch(() => {
    })

    return await response.json()
}

export async function basketGetItemDetails(itemId) {
    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/item/${encodeURIComponent(itemId)}/details?lang=${getSelectedLang()}`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })
    return await response.json()
}

export async function basketGetView() {
    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/view?lang=${getSelectedLang()}`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function basketRemoveItem(itemId) {
    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/item/${encodeURIComponent(itemId)}/remove?lang=${getSelectedLang()}`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store"
    })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function basketUpdateItemNote(itemId, note) {
    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/item/${encodeURIComponent(itemId)}/update-note?lang=${getSelectedLang()}`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            note: note
        })
    })
        .catch(() => {
            push('/error')
        })


    return await response.json()
}

export async function basketIncraseItem(itemId) {
    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/item/${encodeURIComponent(itemId)}/incrase?lang=${getSelectedLang()}`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store"
    })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function basketDecraseItem(itemId) {
    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/item/${encodeURIComponent(itemId)}/decrase?lang=${getSelectedLang()}`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store"
    })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function basketCreateOrder(createOrderRequest) {
    let url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/create-order`

    let headers = {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    }

    let token = get(authData).token
    if (!isEmpty(token)) {
        headers['Authorization'] = 'bearer ' + token
        url += '-for-customer'
    }

    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: headers,
        body: JSON.stringify(createOrderRequest)
    })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function basketCheckPayment(orderId) {
    const url = `${apiUrl}/orders-api/payment/order/${orderId}/check`
    const response = await fetch(url, {
        method: 'GET',
        cache: "no-store",
        headers: {
            'Accept': 'application/json'
        }
    })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

let callbackForRefresh = null

export function setCallbackForRefresh(callback) {
    callbackForRefresh = callback
}

async function basketFireCallbackForRefresh() {
    try {
        if (callbackForRefresh != null) {
            await callbackForRefresh()
        }
    } catch (e) {
    }
}

export async function basketSavePoduct(product) {
    let productToSave = {
        id: product.id,
        note: product.note,
        productId: product.productId,
        subProducts: []
    }

    if (product.productSets != null && product.productSets.length > 0) {
        let groups = product.productSets.reduce((groups, item) => {
            const group = (groups[item.mainProductId] || [])
            group.push(item)
            groups[item.mainProductId] = group
            return groups
        }, {})

        Object.entries(groups).forEach((group) => {
            let productSets = []
            const firstProduct = group[1][0]
            group[1].forEach(productSet => {
                productSet.items.forEach(productSetItem => {
                    if (productSetItem.selected) {
                        productSets.push({
                            productSetId: productSet.id,
                            productSetItemId: productSetItem.id
                        })
                    }
                })
            })

            productToSave.subProducts.push({
                id: firstProduct.mainProductId,
                label: firstProduct.bundleLabel,
                productSets: productSets
            })
        })
    } else {
        productToSave.subProducts.push({
            id: product.productId,
            label: null,
            productSets: []
        })
    }

    if (product.bundles != null && product.bundles.length > 0) {
        product.bundles.forEach(bundle => {
            bundle.items.forEach(bundleItem => {
                if (bundleItem.selected) {
                    productToSave.subProducts.push({
                        id: bundleItem.productId,
                        label: bundle.label,
                        productSets: []
                    })
                }
            })
        })
    }

    if (productToSave.id == null) {
        var response = await basketAddProduct(productToSave)
        if (response.status == 'expired') {
            const initialData = get(basketData)
            basketInit(initialData.restaurantId, initialData.zoneId, initialData.deliveryMode)
                .then(() => {
                    basketAddProduct(productToSave)
                        .then(() => { });
                })
        }
    } else {
        await basketUpdateItem(productToSave.id, productToSave)
    }

    await basketFireCallbackForRefresh()
}

async function basketAddProduct(product) {
    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/add-product?lang=${getSelectedLang()}`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(product)
    })
        .catch(() => {
            push('/error')
        })


    return await response.json()
}

async function basketUpdateItem(itemId, product) {
    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/item/${encodeURIComponent(itemId)}/update?lang=${getSelectedLang()}`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(product)
    })
        .catch(() => {
            push('/error')
        })


    return await response.json()
}

export async function basketSetDiscountCode(code) {
    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/discount-code/apply`
    const response = await fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            code: code
        })
    })
        .catch(() => {
            push('/error')
        })


    var result = await response.json()
    if (result.status == 'ok') {
        basketData.update(state => {
            state.discountCode = code
            state.discountLoaded = true
            return state
        })
    } else {
        basketData.update(state => {
            state.discountCode = null
            state.discountLoaded = false
            return state
        })
    }

    await basketFireCallbackForRefresh()

    return result
}

export async function basketRemoveDiscountCode() {
    const url = `${apiUrl}/orders-api/basket/${encodeURIComponent(get(basketData).basketKey)}/discount-code/remove`
    basketData.update(state => {
        state.discountCode = null
        state.discountLoaded = false
        return state
    })
        .catch(() => {
            push('/error')
        })

    await fetch(url, {
        method: 'POST',
        cache: "no-store",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })

    await basketFireCallbackForRefresh()
}
