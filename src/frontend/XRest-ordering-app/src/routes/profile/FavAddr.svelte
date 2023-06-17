<script>
    import LayoutSimple from '../_LayoutSimple.svelte'
    import ProgressBar  from '../../lib/components/ProgressBar.svelte'
    import AutoComplete from '../../lib/components/Autocomplete.svelte'
    import { _ } from '../../lib/services/localization/i18n'
    import { isEmpty } from '../../lib/helpers'
    import { onMount } from 'svelte'
    import { 
        authData,
        myProfileGetFavoriteAddressList,
        myProfileSaveFavoriteAddress,
        myProfileRemoveFavoriteAddress
     } from '../../lib/services/customer'
    import { searchCity,  searchStreet } from '../../lib/services/api'
    import { push } from '../../lib/router'

    if(!$authData.isLogged) {
        push('/')
    }

    onMount(async () => {
        setTimeout(async () => {
            await loadItems()
        }, 500)
    });

    let showCriticalError = false
    let criticalErrorMessage = null
    let loading = true
    let items = []
    let showEditForm = false
    let showRequiredFiels = false

    async function loadItems() {
        let result = await myProfileGetFavoriteAddressList()
        loading = false
        if (result.status == 'ok') {
            items = result.data
        }
    }

    function newCtx() {
        return {
            id: 0,
            addressCity: null,
            addressStreet: null,
            addressStreetNumber: null,
            addressHouseNumber: null,
            default: false
        }
    }

    let ctx = newCtx()
    
	function clearCity() {
		ctx.addressCity = null
		ctx.addressStreet = null
		ctx.addressStreetNumber = null
	}

    async function searchCityEx(city) {
		if (city == '' || city == null) {
			clearCity()
			return []
		}
		showRequiredFiels = false
		return await searchCity(city)
    }

    async function searchStreetEx(street) {
		return await searchStreet(ctx.addressCity, street)
	}

    async function addNewAddr() {
        ctx = newCtx()

        showEditForm = true
    }
    
    async function saveItem() {
        if (isEmpty(ctx.addressCity) || isEmpty(ctx.addressStreet) || isEmpty(ctx.addressStreetNumber)) {
            showError($_('userProfile.favaddr.enterData'))
            return 
        }

        showEditForm = false
        await myProfileSaveFavoriteAddress(ctx)
        await loadItems()
    }

    async function cancelEdit() {
        showEditForm = false
        ctx = newCtx()
    }

    async function edit(item) {
        ctx = item 
        showEditForm = true
    }

    async function remove(item) {
        await myProfileRemoveFavoriteAddress(item.id)
        await loadItems()
    }

    function showError(text) {
		showCriticalError = true
		criticalErrorMessage = text
	}
</script>
<LayoutSimple>
    <div class="container-fluid">
        <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
            <div class="col-12">
                <img
                    src="assets/img/logo.png"
                    class="mx-auto d-block img-fluid my-5"
                    alt="logo" />
            </div>
        </div>
        
        <div class="mx-auto card my-5 col-md-11 col-xxl-8 col-12">
            <div class="card-body">
                <div class="d-flex d-flex justify-content-between">
                    <h4 class="card-title">{$_("userProfile.favAddr")}</h4>
                    {#if !showEditForm}
                        <button class="btn btn-outline-success shadow" on:click|preventDefault|stopPropagation={addNewAddr}>
                            {$_('userProfile.favaddr.add')}
                        </button>
                    {/if}
                </div>
               
                {#if !showEditForm}
                    <div class="table-responsive">
                        <table class="table">
                            <thead>
                                <tr>
                                <th scope="col">{$_("userProfile.favaddr.default")}</th>
                                <th scope="col">{$_("userProfile.favaddr.addr")}</th>
                                <th scope="col">{$_("userProfile.favaddr.action")}</th>
                                </tr>
                            </thead>
                            <tbody>
                            {#if loading}
                                <tr>
                                    <td colspan="3">
                                        <ProgressBar/>
                                    </td>
                                </tr>
                            {:else}
                                {#if items.length === 0}
                                    <tr>
                                        <td colspan="3" class="text-center">
                                            {$_("userProfile.points.empty")}
                                        </td>
                                    </tr>
                                {:else}
                                    {#each  items as item }
                                        <tr>
                                            <th scope="row text-center">
                                                <input class="form-check-input" type="checkbox"    bind:checked={item.default}  />
                                            </th>
                                            <td>
                                                {item.addressCity}, {item.addressStreet} {item.addressStreetNumber}
                                                {#if  item.addressHouseNumber != null}
                                                    , {item.addressHouseNumber}
                                                {/if}                                
                                            </td>
                                            <td>
                                                <button  title="{$_("userProfile.favaddr.edit")}" class="btn btn-sm btn-light btn-basket" on:click={() => edit(item)}><i class="fas fa-edit" /></button>
                                                <button data-toggle="tooltip" title="{$_("userProfile.favaddr.delete")}" class="btn btn-sm btn-light btn-basket" on:click={() => remove(item)}><i class="fas fa-trash-alt" /></button>
                                            </td>
                                        </tr>
                                    {/each}
                                
                                {/if}
                            {/if}
                            </tbody>
                        
                        </table>  
                    </div>
                {:else}

                    {#if showCriticalError}
                        <div class="alert alert-danger" role="alert">
                            {criticalErrorMessage}
                        </div>
                    {/if}
                    <div class="row">
                        <div class="my-3 col-4">
                             <AutoComplete
                                searchFunction={searchCityEx}
                                clearFunction={clearCity}
                                bind:selectedItem={ctx.addressCity}
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
                                placeholder={$_('search.placeHolderCity')}/>
                        </div>
                        <div class="my-3 col-4">
                            <AutoComplete
                                searchFunction={searchStreetEx}
                                bind:selectedItem={ctx.addressStreet}
                                class="form-control shadow"
                                maxItemsToShowInList="10"
                                delay="200"
                                showClear={true}
                                disabled={ctx.addressCity == null}
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
                        <div class="my-3 col-2">
                            <input
                                type="text"
                                name="numer"
                                placeholder={$_('search.placeHolderStreetNumber')}
                                class="form-control inp shadow {showRequiredFiels
                                    ? 'is-invalid'
                                    : ''}"
                                bind:value={ctx.addressStreetNumber}
                            />
                        </div>
                        <div class="my-3 col-2">
                            <input
                                type="text"
                                name="lokal"
                                placeholder={$_('search.placeHolderHouseNumber')}
                                class="form-control shadow inp"
                                bind:value={ctx.addressHouseNumber}
                            />
                        </div>
                    </div>
                    <div class="d-flex d-flex justify-content-start">
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox"  name="def"  bind:checked={ctx.default}  id="def" required>
                            <label class="form-check-label"  for="def">
                                {$_('userProfile.favaddr.def')}
                            </label>
                        </div>
                    </div>
                    <div class="d-flex d-flex justify-content-end">
                        <button class="btn btn-outline-success  me-2" on:click|preventDefault|stopPropagation={saveItem}>
                            {$_('userProfile.favaddr.save')}
                        </button>                       
                        <button class="btn btn-outline-danger " on:click|preventDefault|stopPropagation={cancelEdit}>
                            {$_('userProfile.favaddr.cancel')}
                        </button>
                    </div>
                {/if}  
        </div>
    </div>
</LayoutSimple>
<style>
.inp { 
    height: 46px;
    width: 150px;
}
</style>