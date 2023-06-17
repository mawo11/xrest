<script>
	import { onDestroy } from 'svelte'
	import { selectedRestaurant } from '../../stores'
	import { _, locale } from '../../services/localization/i18n'
	import {
		getMenu,
		getProductsFromGroup,
		getImageUrl,
		getProductDetails,
	} from '../../services/api'
	import ProgressBar from '../ProgressBar.svelte'
	import { configuredProduct } from './ConfiguratorDialog.svelte'
	

	let loadingData = true;
	let menu = {
		groups: [],
		products: [],
	}

	let currentGroupId  = 0
	
	let unsubcribeSelectedRestaurant = selectedRestaurant.subscribe( (value) => {
		if (value.loaded) {
			getMenu($selectedRestaurant.id).then((result) => {
				menu = result;
				currentGroupId = menu.groups[0].id
				loadingData = false
			})
		}
	})

	let unsubscribeLocale = locale.subscribe( (val) =>{
		getMenu($selectedRestaurant.id).then((result) => {
				menu = result;
				currentGroupId = menu.groups[0].id
				loadingData = false
			})
	})

	onDestroy(() => {
		try {
			unsubcribeSelectedRestaurant()
			unsubscribeLocale()
		} catch (e) {}
	})

	async function loadGroup(id) {
		currentGroupId = id
		menu.products = await getProductsFromGroup($selectedRestaurant.id, id)
	}

	async function configure(product) {
		try {
			let currentProduct = await getProductDetails(product.id)
			if (currentProduct.notFound) {
				return
			}

			configuredProduct.set(currentProduct)
		} catch (e) {
			console.debug(e)
			return
		}		
	}
	
</script>
{#if loadingData}
	<ProgressBar />
{:else}
	<div class="row w-100 mx-auto py-1 pb-4">
		{#each menu.groups as group}
			<button class="btn btn-kebab mx-1 col-sm  text-nowrap mb-2  {group.id == currentGroupId ? 'btn-kebab-active': ''} " 
				on:click|preventDefault|stopPropagation={loadGroup(group.id)}>{group.name}</button>
		{/each}
	</div>

	<div class="row row-cols-1 row-cols-md-3 row-cols-xxl-4 row-col-1 g-4">
		{#each menu.products as product}
		<div class="col">
			<div class="card h-100">
				<img src={getImageUrl(product.imageUrl)} class="card-img-top" alt="...">
				<div class="card-body">
				<h5 class="card-title">{product.name}</h5>
				<p class="card-text">{product.description}</p>
				</div>
				<div class="card-footer bg-white d-grid gap-2">
					<h5 class="text-center ">{product.price}</h5>
					{#if $selectedRestaurant.working}
						<button on:click|preventDefault|stopPropagation={configure(product)} class="btn btn-dark">{$_("menu.add")}</button>
					{/if}
				</div>
			</div>
		</div>
		{/each}
	</div>
{/if}