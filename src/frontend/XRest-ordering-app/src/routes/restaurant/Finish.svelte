<script>
	import CommonLayout from './_CommonLayout.svelte'
	import { _ } from '../../lib/services/localization/i18n'
	import { selectedRestaurant, } from '../../lib/stores'
	import { basketData, basketClear } from '../../lib/services/basketApi'
    import { routeParams } from '../../lib/router'

    let orderId = $routeParams.orderId

	let deliveryMode= $basketData.deliveryMode
	basketClear()
</script>

<CommonLayout>
	<div class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
		<div class="col-12 justify-content-center text-center fs-1 text-light my-4 py-4" style="text-shadow: 4px 4px 5px rgb(48, 45, 47);">
			<span class="text-uppercase display-4 fw-bold">{$_("finish.title")}</span>
		</div>
	</div>
	<div class="row mx-auto py-2 col-12 col-xxl-9 col-xl-12">
		<div class="col-xxl-12 col-sm-12 col-12 col-md-12 order-xxl-1 order-xl-1 order-2 text-center m-5">
			<p class="h2">{$_("payment.orderNo")} {orderId} </p>
			<br/>
			{#if deliveryMode == 1 }
				<img src="/assets/img/take-away.png" class="img-fluid" alt="delivery" />
			{:else}
				<img src="/assets/img/delivery.gif" class="img-fluid" alt="delivery" />
			{/if}
			
			<br/><br/><br/>
			{#if deliveryMode == 1 }
				<p class="h2">{$_("finish.thanksPerson")}</p>
			{:else}
				<p class="h2">{$_("finish.thanksDelivey")}</p>
				<p class="h2">{$_("finish.deliveryTime")} {$selectedRestaurant.realizationTime} min.</p>
			{/if}
		</div>
	</div>
</CommonLayout>
<style>
	.img-fluid { 
		width: 200px  !important;
	}
</style>