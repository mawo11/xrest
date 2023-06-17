<script>
	import CommonLayout from './_CommonLayout.svelte'
	import { _ } from "../../lib/services/localization/i18n"
	import { onMount } from 'svelte'
	import { 
		selectedRestaurant,
	} from '../../lib/stores'
	import { 
		basketCheckPayment,
		basketClear
	} from '../../lib/services/basketApi'
    import { routeParams } from '../../lib/router'

    let orderId = $routeParams.orderId

	onMount(() => {
		basketClear()
		checkForPaymentStatus();
	})

	let status = 'waiting'

	function checkForPaymentStatus() {
		basketCheckPayment(orderId)
			.then( (result) => {
				status = result.status
				if (result.status === 'waiting'){
					setTimeout(() => {
							checkForPaymentStatus()
						}, 1000)
				}
			})
	}

	async function checkPaymentStatus() {
		basketCheckPayment(orderId)
			.then( (result) => {
				status = result.status
			})
	}
</script>
<CommonLayout>
	<div class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
		<div class="col-12 justify-content-center text-center fs-1 text-light my-4 py-4" style="text-shadow: 4px 4px 5px rgb(48, 45, 47);">
			<span class="text-uppercase display-4 fw-bold">{$_("payment.title")}</span>
		</div>
	</div>
	<div class="row mx-auto py-2 col-12 col-xxl-9 col-xl-12">
		<div class="col-xxl-12 col-sm-12 col-12 col-md-12 order-xxl-1 order-xl-1 order-2 text-center m-5">
			<p class="h2">{$_("payment.orderNo")} {orderId} </p>
			<br/>
			{#if status == 'waiting'}
				<img
					src="assets/img/clock.png" class="img-fluid heartbeat"
					alt="delivery"
					/>
				<br/><br/><br/>
				<p class="h2">{$_("payment.waiting")}</p>
				<br/>
				<button
					type="input"
					on:click|preventDefault|stopPropagation={checkPaymentStatus}
					class="btn kk-color text-uppercase text-white shadow">
					{$_('payment.checkPaymentStatus')}
				</button> 
				
			{/if}
			{#if status == 'error'}
				<br/>
				<p class="h2">{$_("payment.error")}</p>
			{/if}
			{#if status == 'ok'}
				<img
						src="assets/img/delivery.png" class="img-fluid mini"
						alt="delivery"
					/>
				<br/><br/><br/>
				<p class="h2">{$_("payment.thanks")}</p>
				<p class="h2">{$_("payment.deliveryTime")} {$selectedRestaurant.realizationTime || 90 } min.</p>
			{/if}
		</div>
	</div>

</CommonLayout>
<style>
	.img-fluid { 
		width: 200px  !important;
	}
.heartbeat {
	-webkit-animation: heartbeat 1.5s ease-in-out infinite both;
	        animation: heartbeat 1.5s ease-in-out infinite both;
}

 @-webkit-keyframes heartbeat {
  from {
    -webkit-transform: scale(1);
            transform: scale(1);
    -webkit-transform-origin: center center;
            transform-origin: center center;
    -webkit-animation-timing-function: ease-out;
            animation-timing-function: ease-out;
  }
  10% {
    -webkit-transform: scale(0.91);
            transform: scale(0.91);
    -webkit-animation-timing-function: ease-in;
            animation-timing-function: ease-in;
  }
  17% {
    -webkit-transform: scale(0.98);
            transform: scale(0.98);
    -webkit-animation-timing-function: ease-out;
            animation-timing-function: ease-out;
  }
  33% {
    -webkit-transform: scale(0.87);
            transform: scale(0.87);
    -webkit-animation-timing-function: ease-in;
            animation-timing-function: ease-in;
  }
  45% {
    -webkit-transform: scale(1);
            transform: scale(1);
    -webkit-animation-timing-function: ease-out;
            animation-timing-function: ease-out;
  }
}
@keyframes heartbeat {
  from {
    -webkit-transform: scale(1);
            transform: scale(1);
    -webkit-transform-origin: center center;
            transform-origin: center center;
    -webkit-animation-timing-function: ease-out;
            animation-timing-function: ease-out;
  }
  10% {
    -webkit-transform: scale(0.91);
            transform: scale(0.91);
    -webkit-animation-timing-function: ease-in;
            animation-timing-function: ease-in;
  }
  17% {
    -webkit-transform: scale(0.98);
            transform: scale(0.98);
    -webkit-animation-timing-function: ease-out;
            animation-timing-function: ease-out;
  }
  33% {
    -webkit-transform: scale(0.87);
            transform: scale(0.87);
    -webkit-animation-timing-function: ease-in;
            animation-timing-function: ease-in;
  }
  45% {
    -webkit-transform: scale(1);
            transform: scale(1);
    -webkit-animation-timing-function: ease-out;
            animation-timing-function: ease-out;
  }
}


</style>