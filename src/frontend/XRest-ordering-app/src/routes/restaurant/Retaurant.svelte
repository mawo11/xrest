<script>
	import Basket from '../../lib/components/restaurant/Basket.svelte'
	import Menu from '../../lib/components/restaurant/Menu.svelte'
	import RestaurantInfo from '../../lib/components/restaurant/RestaurantInfo.svelte'
	import ConfiguratorDialog from '../../lib/components/restaurant/ConfiguratorDialog.svelte'
	import ProgressBar from '../../lib/components/ProgressBar.svelte'
	import HeaderFull from '../../lib/components/HeaderFull.svelte'
	import { selectedRestaurant } from '../../lib/stores'
	import { onMount } from 'svelte'
	import { getRestaurantByAlias } from '../../lib/services/api'
	import { _ } from '../../lib/services/localization/i18n'
	import { basketInit } from '../../lib/services/basketApi'
	import { routeParams, push } from '../../lib/router'

	let alias = $routeParams.alias
	let zoneId =$routeParams.zoneId
	let deliveryMode = $routeParams.deliveryMode

	let shown = false

	onMount(() => {
	
		if (!$selectedRestaurant.loaded) {

			if(deliveryMode == 0) {
				push('/')
				return
			}

			setTimeout(() => {
				getRestaurantByAlias(alias).then((restaurant) => {
					if (!restaurant.found) {
						push('/')
						return
					}

					restaurant.loaded = true
					selectedRestaurant.set(restaurant)
				    basketInit(restaurant.id, zoneId, deliveryMode)
						.then(() => {
							shown = true
						})	
				})
				shown = true
			}, 500)
		} else {
			basketInit($selectedRestaurant.id, zoneId, deliveryMode)
				.then(() => {
					shown = true
				});	
			
		}
	})

	$: document.title = $_("pageTitle") + (shown ? ' - ' + $selectedRestaurant.name : '')
</script>

<HeaderFull />
<main class="container-fluid">
	{#if shown}
		<div class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
			<div class="col-12 justify-content-center text-center fs-1 text-light my-4 py-4" style="text-shadow: 4px 4px 5px rgb(48, 45, 47);">
				<span class="text-uppercase display-4 fw-bold">{$selectedRestaurant.name}</span>
			</div>
		</div>
		<div class="row mx-auto py-2 col-12 col-xxl-9 col-xl-12">
			<div class="col-xxl-8 col-sm-12 col-12 col-md-8 order-xxl-1 order-xl-1 order-2">
				<Menu />
			</div>
			<div class="col-12 col-sm-4 col-xxl-4 col-xl-4 order-xxl-2 order-xl-2 order-1 justify-content-center align-items-center px-xxl-5 px-xl-4">
				<RestaurantInfo />
				<Basket />
			</div>
		</div>
		<ConfiguratorDialog />
	{/if}
	{#if !shown}
		<ProgressBar/>
	{/if}
</main>

<style>
	:global(body) {
		overflow-y: scroll !important;
		width: 100%;
	}

</style>
