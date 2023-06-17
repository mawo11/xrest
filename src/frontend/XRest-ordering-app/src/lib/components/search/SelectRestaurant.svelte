<script>
    import AutoComplete from '../Autocomplete.svelte'
	import { _ } from '../../services/localization/i18n'
	import { onMount } from 'svelte'
    import { push } from '../../router'
    import { searchCity, searchStreet, searchRestaurant, getAllRestaurants } from '../../services/api'
    import { basketInit } from '../../services/basketApi'
    import { selectedRestaurant, selectedAddress } from '../../stores'

    let restaurants = []
	let city = '',
		street = '',
		streetNumber = '',
		houseNumber = ''
    let showRequiredFiels = false
    let showNotFound = false

    onMount(() => {
        getAllRestaurants()
            .then(result =>{
                restaurants = result.restaurants
            })
    })

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

	async function findRestaurant() {
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
       
        const restaurant = await searchRestaurant(city, street, streetNumber, true)
        if (restaurant.found) {
            selectedAddress.set({
                city: city,
                street: street,
                streetNumber: streetNumber,
                houseNumber: houseNumber
            })

            restaurant.loaded = true
            selectedRestaurant.set(restaurant)
            basketInit(restaurant.id, restaurant.transportZoneID, 1)
                .then(() => {
                    push(`/restaurant/${restaurant.alias}/${restaurant.transportZoneID}/1`)
                })
        } else {
            showNotFound = true
        }
	}
    
    function handleSubmit(e) {
    }
    
    async function selectRestaurant(restaurant){
        selectedRestaurant.set(restaurant)
        basketInit(restaurant.id, restaurant.transportZoneID, 1)
						.then(() => {
							push( `/restaurant/${restaurant.alias}/${restaurant.transportZoneID}/1`)
						})
    }
</script>
<div class="row">
    <div class="col">
        <h3 class="text-success mb-3 pick_up-title">{$_('search.takeAwayTitle1')}</h3>
        <ul class="list-group list-group-flush mt-5 mb-3" style="max-height: 300px;  overflow-y:scroll;">
            {#each  restaurants as restaurant }
            <!-- svelte-ignore a11y-invalid-attribute -->
            <a class="list-group-item list-group-item-action" href="#"  on:click|preventDefault|stopPropagation={() =>selectRestaurant(restaurant)}>
                <h4>{restaurant.name}</h4>
                {restaurant.city}, {restaurant.address}
            </a>
            {/each}
          </ul>
    </div>
    <div class="col">
        <h3 class="text-success text-end mb-3 pick_up-title">{$_('search.takeAwayTitle2')}</h3>
        <form on:submit|preventDefault={handleSubmit}>
            <div class="row mb-4 mt-5">
                <div class="col-12 input-group-lg my-1">
                            <AutoComplete
                                searchFunction={searchCityEx}
                                clearFunction={clearCity}
                                bind:selectedItem={city}
                                class="form-control shadow"
                                maxItemsToShowInList="10"
                                delay="200"
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
                <div class="col-12 input-group-lg my-1">
                         <AutoComplete
                            searchFunction={searchStreetEx}
                            bind:selectedItem={street}
                            class="form-control shadow"
                            maxItemsToShowInList="10"
                            delay="200"
                            showClear={true}
                            disabled={city == ''}
                            showLoadingIndicator={true}
                            loadingText={$_('search.loading')}
                            localFiltering={false}
                            inputClassName={showRequiredFiels
                                ? 'is-invalid'
                                : ''}
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
            </div>
            {#if showNotFound}
                <div class="row justify-content-start">
                    <div class="col-12 col-md-12 input-group-lg alert alert-danger text-center">
                        {$_('search.notFound')}
                    </div>
                </div>
            {/if}
            <div class="input-group input-group-lg mt-5 justify-content-end">
                <button type="input" 
                on:click|preventDefault|stopPropagation={findRestaurant}
                class="btn kk-color text-uppercase text-white shadow">
                    <i class='fas fa-search-location'></i>
                    {$_('search.findRest')}
                </button>	
            </div>
          </form>
    </div>
</div>