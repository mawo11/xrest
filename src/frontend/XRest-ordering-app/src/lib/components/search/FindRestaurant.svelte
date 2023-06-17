<script>
	//TODO: wyszukiwania restauracji, moze jakis kreciolek? a potem przejscie do nowej restauracji
	import { _ } from '../../services/localization/i18n'
	import AutoComplete from '../Autocomplete.svelte'
    import { onMount } from 'svelte'
	import {
		searchCity, 
		searchStreet,
		searchRestaurant,
	} from '../../services/api'
    import {
		authData,
        myProfileGetFavoriteAddressList
	} from '../../services/customer'
    import { basketInit } from '../../services/basketApi'
    import { isEmpty } from '../../helpers'
	import { selectedRestaurant, selectedAddress } from '../../stores'
    import { push } from '../../router'

	let city = '',
		street = '',
		streetNumber = '',
		houseNumber = ''
	let showRequiredFiels = false

	async function searchCityEx(city) {
		if (city == '' || city == null) {
			clearCity()
			return []
		}
		showRequiredFiels = false
		return await searchCity(city)
	}

	async function searchStreetEx(street) {
		return await searchStreet(city, street)
	}

	function clearCity() {
		city = ''
		street = ''
		streetNumber = ''
		houseNumber = ''
		showNotFound = false
		showRequiredFiels = false
	}

    let showNotFound = false

	async function findRestaurant() {
        showNotFound = false
		showRequiredFiels =
			city == '' ||
			city == null ||
			street == '' ||
			street == null ||
			streetNumber == '' ||
			streetNumber == null
        if (showRequiredFiels) {
            return
        }

		const restaurant = await searchRestaurant(city, street, streetNumber, false)
        if (restaurant.found) {
            selectedAddress.set({
                city: city,
                street: street,
                streetNumber: streetNumber,
                houseNumber: houseNumber
            })

            restaurant.loaded = true
            selectedRestaurant.set(restaurant)
            basketInit(restaurant.id, restaurant.transportZoneID, 0)
						.then(() => {
							push(`/restaurant/${restaurant.alias}/${restaurant.transportZoneID}/0`)
						})
        } else {
            showNotFound = true
        }
	}
    
    function handleSubmit(e) {
    }

    let addressList = [{
        id: -1,
        text: $_('search.defAddr'),
        selected: false,
        ctx: null
    }] 
    let selectedAddrId =  -1 
    function selectAddressFromListbox() {
        if (selectedAddrId == -1) {
            city =''
            street = ''
            streetNumber = ''
            houseNumber = ''
            return
        }

        addressList.forEach( item => {
            if (item.id == selectedAddrId) {
                selectAddress(item.ctx)
            }
        })
    }
    
    function selectAddress(item) {
       
        city = item.addressCity
		street = item.addressStreet
		streetNumber = item.addressStreetNumber
		houseNumber = item.addressHouseNumber
    }

    onMount(async () => {
        if ($authData.isLogged ) {
            const result  = await myProfileGetFavoriteAddressList()
            if (result.status == 'ok') {
                let defautExists = false

                result.data.forEach( item => {
                    let text = `${item.addressCity}, ${item.addressStreet} ${item.addressStreetNumber}`
                    if (!isEmpty(item.addressHouseNumber)) {
                        text += ', ' + item.addressHouseNumber
                    }
                        
                    if (item.default) { 
                        defautExists = true
                        selectAddress(item)
                        selectedAddrId = item.id
                    }

                    addressList.push({
                        id:  item.id,
                        text: text,
                        selected: item.default,
                        ctx: item
                    })
                })

                if (!defautExists) {
                    addressList[0].selected = true
                }

                addressList = addressList
            }
            
        }
    })
</script>
<form on:submit|preventDefault={handleSubmit}>
    <div class="row mb-4">
        <div class="col-md-4 col-12 input-group-lg">
            <AutoComplete
                searchFunction={searchCityEx}
                clearFunction={clearCity}
                bind:selectedItem={city}
                class='form-control shadow'
                maxItemsToShowInList='10'
                delay='200'
                showClear={true}
                showLoadingIndicator={true}
                localFiltering={false}
                loadingText={$_('search.loading')}
                inputClassName={showRequiredFiels
                    ? 'is-invalid'
                    : null}
                html5autocomplete={false}
                placeholder={$_('search.placeHolderCity')}
            />
        </div>
        <div class="col-md-4 col-12 input-group-lg">
            <AutoComplete
                searchFunction={searchStreetEx}
                bind:selectedItem={street}
                class='form-control'
                maxItemsToShowInList='10'
                delay='200'
                showClear={true}
                disabled={city == ''}
                showLoadingIndicator={true}
                loadingText={$_('search.loading')}
                localFiltering={false}
                inputClassName='form-control shadow {showRequiredFiels
                    ? 'is-invalid '
                    : ''}'
                html5autocomplete={false}
                placeholder={$_('search.placeHolderStreet')}
            />
        </div>
        <div class="col-md-2 col-6 input-group-lg">
            <input
                type="text"
                name="numer"
                placeholder={$_('search.placeHolderStreetNumber')}
                class="form-control shadow {showRequiredFiels
                    ? 'is-invalid'
                    : ''}"
                bind:value={streetNumber}
            />
        </div>
        <div class="col-md-2 col-6 input-group-lg">
            <input
                type="text"
                name="lokal"
                placeholder={$_('search.placeHolderHouseNumber')}
                class="form-control shadow"
                bind:value={houseNumber}
            />
        </div>
    </div>
    {#if showNotFound}
        <div class="row justify-content-start">
            <div class="col-12 col-md-12 input-group-lg alert alert-danger text-center">
                {$_('search.notFound')}
            </div>
        </div>
    {/if}
    {#if $authData.isLogged && addressList!=null && addressList.length > 1}
        <div class="row justify-content-start">
            <h5 class="pick_up-title text-success mx-2">lub</h5>
            
            <div class="col-12 col-md-12 input-group-lg">
                <select class="form-control my-2 shadow" bind:value={selectedAddrId} on:change={selectAddressFromListbox}>
                   {#each addressList as addr }
                       <option value={addr.id} selected={addr.selected}>{addr.text}</option>
                   {/each}
                </select>
            </div>
        </div>
    {/if}
    <div class="input-group input-group-lg mt-5 justify-content-end" >
        <button
            type="input"
            on:click|preventDefault|stopPropagation={findRestaurant}
            class="btn kk-color text-uppercase text-white shadow">
            <i class="fas fa-search-location" />
            {$_('search.findRest')}
        </button> 
    </div>
</form>