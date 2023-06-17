<script>
	import { _ } from '../lib/services/localization/i18n'
	import LayoutSimple from './_LayoutSimple.svelte'
	import SelectRestaurant from '../lib/components/search/SelectRestaurant.svelte'
	import FindRestaurant from '../lib/components/search/FindRestaurant.svelte'

	let selectedTab = 0
    import { basketClear } from '../lib/services/basketApi'
	import { clearData } from '../lib/stores'

	basketClear()
	clearData()
</script>
<LayoutSimple>
	<div class="container-fluid">
		<div class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
			<div class="col-12">
				<img src="assets/img/logo.png" class="mx-auto d-block img-fluid my-5" alt="logo" />
			</div>
		</div>
		<div class="mx-auto card my-5 col-md-11 col-xxl-7">
			<div class="card-header bg-white">
				<ul class="nav card-header-pills justify-content-center fs-4 search-address">
					<li class="nav-item text-uppercase">
						<!-- svelte-ignore a11y-invalid-attribute -->
						<a class="nav-link {selectedTab == 0 ? 'active link-success fw-bold' : 'link-dark'} "
						on:click={()=>selectedTab = 0}
							href="#">{$_("search.delivey")}</a>
					</li>
					<li>
						<hr class="vert-line hr" />
					</li>
					<li class="nav-item text-uppercase">
						<!-- svelte-ignore a11y-invalid-attribute -->
						<a class="nav-link {selectedTab == 1 ? 'active link-success fw-bold' : 'link-dark'}" href="#" 
						on:click={()=>selectedTab = 1}>{$_("search.personal")}</a>
					</li>
				</ul>
			</div>
			<div class="card-body">
				{#if selectedTab == 0} 
					<FindRestaurant />
				{/if}
				{#if  selectedTab == 1}
					<SelectRestaurant />
				{/if}
			</div>
		</div>
	</div>
</LayoutSimple>

<style>
	:global(.input_error) {
		background-color: red !important;
		color: white !important;
	}
	.vert-line {
		rotate: (90deg); 
		height: 45px; 
		border: 2px solid green; 
		margin-top: 5px;
	}
</style>