import { writable } from 'svelte/store'
import { getObjectFromSessionStorage } from './helpers'

let storedRestaurantData = getObjectFromSessionStorage('restaurant');

export const selectedRestaurant = writable(storedRestaurantData || {
    loaded: false,
    name: '',
    address: '',
    postCode: '',
    city: '',
    onlineFrom: '',
    onlineTo: '',
    working: false,
    found: false,
    id: 0,
    realizationTime: 0
});

selectedRestaurant.subscribe(value => {
    sessionStorage.setItem('restaurant', JSON.stringify(value))
})

let storedAddressData = getObjectFromSessionStorage('address');

export const selectedAddress = writable(storedAddressData || {
    city: '',
    street: '',
    streetNumber: '',
    houseNumber: ''
});

selectedAddress.subscribe(value => {
    sessionStorage.setItem('address', JSON.stringify(value))
});


export function clearData() {
    sessionStorage.removeItem('address');
    sessionStorage.removeItem('restaurant');
}

export function restDataRefreshFromSession() {
    let storedRestaurantData = getObjectFromSessionStorage('restaurant')
    selectedRestaurant.set(storedRestaurantData)
}