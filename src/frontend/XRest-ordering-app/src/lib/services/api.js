import { authData } from './customer'
import { get } from 'svelte/store'
import { isEmpty, getSelectedLang } from '../helpers'
import { push } from '../router'

export async function searchCity(city) {
    const url = `${apiUrl}/restaurants-api/location/find-cities?city=${encodeURIComponent(city)}`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function searchStreet(city, street) {
    const url = `${apiUrl}/restaurants-api/location/find-streets?city=${encodeURIComponent(city)}&street=${encodeURIComponent(street)}`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function searchRestaurant(city, street, streetNumber, isPosCheckout) {
    const url = `${apiUrl}/restaurants-api/location/find-restaurant?city=${encodeURIComponent(city)}&street=${encodeURIComponent(street)}&streetNumber=${encodeURIComponent(streetNumber)}&isPosCheckout=${encodeURIComponent(isPosCheckout)}`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function getRestaurantByAlias(alias) {
    const url = `${apiUrl}/restaurants-api/restaurant/get-by-alias?alias=${encodeURIComponent(alias)}`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function getMenu(restaurantId) {
    const url = `${apiUrl}/orders-api/menu/restaurant/${encodeURIComponent(restaurantId)}/menu?lang=${getSelectedLang()}`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function getProductsFromGroup(restaurantId, groupId) {
    const url = `${apiUrl}/orders-api/menu/restaurant/${encodeURIComponent(restaurantId)}/group/${encodeURIComponent(groupId)}/products?lang=${getSelectedLang()}`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export function getImageUrl(imageId) {
    return `${apiUrl}/images-api/images/product?fileid=${imageId}`
}

export async function getProductDetails(productId) {
    const url = `${apiUrl}/orders-api/menu/product/${encodeURIComponent(productId)}/details?lang=${getSelectedLang()}`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function getAllRestaurants() {
    const url = `${apiUrl}/restaurants-api/restaurant/list`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function getAllowedPayments(restaurantId) {
    const url = `${apiUrl}/restaurants-api/payments/restaurant/${restaurantId}/list?lang=${getSelectedLang()}`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function getTerms() {
    const url = `${apiUrl}/identity-api/terms?lang=${getSelectedLang()}`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}

export async function getRestaurantTerms(restaurantId) {
    const url = `${apiUrl}/restaurants-api/restaurant/${restaurantId}/terms`
    const response = await fetch(url, { cache: "no-store" })
        .catch(() => {
            push('/error')
        })

    return await response.json()
}
