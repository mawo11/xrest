<script>
	import CommonLayout from './_CommonLayout.svelte'
	import RestaurantInfo from '../../lib/components/restaurant/RestaurantInfo.svelte'
	import Basket from '../../lib/components/restaurant/Basket.svelte'
	import ProgressBar from '../../lib/components/ProgressBar.svelte'
	import Terms from '../../lib/components/Terms.svelte'
	import { _, locale } from '../../lib/services/localization/i18n'
	import { onDestroy } from 'svelte'
	import { push } from '../../lib/router'
	import { selectedRestaurant, selectedAddress } from '../../lib/stores'
	import { 
		basketData,
		basketCreateOrder,
		basketSetDiscountCode,
		basketRemoveDiscountCode,
		basketRefreshFromSession
	} from '../../lib/services/basketApi'
	import { getAllowedPayments, getTerms, } from '../../lib/services/api'
	import { isEmpty } from '../../lib/helpers'
	import { authData } from '../../lib/services/customer'
	let payments = []
	
	basketRefreshFromSession();

	let unsubscribeLocale = locale.subscribe( (val) =>{
		getAllowedPayments($basketData.restaurantId)
			.then(result => payments = result)
		getTerms()
		.then(result => terms = result)
	})

	onDestroy(() => {
		try {
			unsubscribeLocale()
		} catch (e) {}
	})


	getAllowedPayments($basketData.restaurantId)
		.then(result => payments = result)

	let terms = []
	getTerms()
		.then(result => terms = result)

    let ctx = {
		paymentTypeId: -1,
		paymentTypeIdError: false,
		requiredTerms: false,
		requiredTermsError: false,
		termSelection: [],
		firstname: null,
		firstnameError: false,
		phone: null,
		phoneError: false,
		email: null,
		emailError: false,
		invoice: false,
		invoiceNip: null,
		invoiceNipError: false,
		invoiceName: null,
		invoiceNameError: false,
		invoiceAddr: null,
		invoiceAddrError: false
	}

	if ($authData.isLogged) {
		ctx.email = $authData.email
		ctx.phone = $authData.phone
		ctx.firstname = $authData.firstname
	}

	function checkForRequired() {
		ctx.phoneError = isEmpty(ctx.phone)
		ctx.emailError = isEmpty(ctx.email) || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(ctx.email)
		ctx.firstnameError = isEmpty(ctx.firstname)
		ctx.paymentTypeIdError = ctx.paymentTypeId == -1
		ctx.requiredTermsError = !ctx.requiredTerms
		
		if (ctx.invoice) {
			ctx.invoiceNipError = isEmpty(ctx.invoiceNip)
			ctx.invoiceNameError = isEmpty(ctx.invoiceName)
			ctx.invoiceAddrError = isEmpty(ctx.invoiceAddr)
		} else {
			ctx.invoiceNipError = false
			ctx.invoiceNameError = false
			ctx.invoiceAddrError = false
		}

		return  ctx.phoneError ||  ctx.emailError ||  ctx.firstnameError || ctx.paymentTypeIdError || ctx.requiredTermsError
				|| (ctx.invoice && (ctx.invoiceNipError || ctx.invoiceNameError || ctx.invoiceAddrError))
	}

	async function goBack() {
		push(`/restaurant/${$selectedRestaurant.alias}/${$basketData.zoneId}/${$basketData.deliveryMode}`)
	}

	let code = $basketData.discountCode
	let showInvalidCodeAlert = false
	async function applyDiscountCode() {
		showInvalidCodeAlert = false
		if (code == null || code.length ==0 ){
			//TODO: alert
			return
		}

		var result = await basketSetDiscountCode(code)
		if (result.status == 'invalidCode') {
			showInvalidCodeAlert = true
		}
	}

	async function removeDiscountCode() {
		code = null
		await basketRemoveDiscountCode()
	}

	let processing = false
	let sending = false
	async function goCreateOrder() {
		if (processing) {
			return
		}

		processing = true
		showCriticalError = false

		if (checkForRequired()) {
			processing = false
			return
		}

		sending = true
	
		let request = {
			email: ctx.email,
			phone: ctx.phone,
			note: ctx.note,
			firstname: ctx.firstname,
			paymentId: ctx.paymentTypeId,
			termOfUse:  ctx.requiredTerms,
			MarketingIds: ctx.termSelection || []
		}

		if ( $basketData.deliveryMode == 0) {
			request.delivery = {
				city: $selectedAddress.city,
				street: $selectedAddress.street,
				streetNumber: $selectedAddress.streetNumber,
				houseNumber: $selectedAddress.houseNumber
			}
		}

		if (ctx.invoice) {
			request.invoice = {
				nip: ctx.invoiceNip,
				name: ctx.invoiceName,
				address: ctx.invoiceAddr
			}
		}
		
		var response = await basketCreateOrder(request)
		processing = false
		switch (response.status) {
			case 'invalidNip':
				showError($_('summary.errors.invalidNip'))
				break
			case 'invalidOrder':
				showError($_('summary.errors.invalidOrder'))
				break
			case 'invalidAddress':
				showError($_('summary.errors.invalidAddress'))
				break
			case 'invalidPhone':
				ctx.phoneError = true
				showError($_('summary.errors.invalidPhone'))
				break
			case 'goToOnlinePayment':
				document.location = response.data
				break
				
			case 'onlinePaymentError':
				push('/payment-error') 
				break;
			case 'ok':
				push(`/finish/${response.orderId}`)
				break
			case 'unknownError':
			default:
				showError($_('summary.errors.unknown'))
				break
		}
	}

	let showCriticalError = false
	let criticalErrorMessage = null
	function showError(text) {
		sending = false
		showCriticalError = true
		criticalErrorMessage = text
	}
</script>

<CommonLayout>
	<div class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
		<div class="col-12 justify-content-center text-center fs-1 text-light my-4 py-4" style="text-shadow: 4px 4px 5px rgb(48, 45, 47);">
			<span class="text-uppercase display-4 fw-bold">{$_("summary.title")}</span>
		</div>
	</div>
	<div class="row mx-auto py-2 col-12 col-xxl-9 col-xl-12">
		<div class="col-xxl-8 col-sm-12 col-12 col-md-8 order-xxl-1 order-xl-1 order-2">
			{#if showCriticalError}
				<div class="alert alert-danger" role="alert">
					{criticalErrorMessage}
				</div>
			{/if}
			{#if sending}
			<div class="row py-1 mx-1 my-2 border border-1 bg-white d-flex justify-content-center align-items-center sending-box" style="border-color: darkgrey">
				{$_("summary.sendingOrder")}
				<br/>
				<ProgressBar />
			</div>
			{:else}
				<div class="row py-1 mx-1 my-2 border border-1 bg-white" style="border-color: darkgrey">
					<h3 class="pt-3">{$_("summary.yoursData")}</h3>
					<div class="row my-2">
						<div class="col-12 col-md-4 input-group-lg">
							<input type="text" class="form-control mx-1 shadow my-1 {ctx.firstnameError ? 'is-invalid' : null}" bind:value={ctx.firstname} placeholder={$_("summary.firstname")} name="firstname" required>
							{#if ctx.firstnameError}
								<div class="invalid-feedback">{$_("summary.validationErrors.firstname")}</div>
							{/if}
						</div>
						<div class="col-12 col-md-4 input-group-lg">
							<input type="text" class="form-control mx-1 shadow my-1 {ctx.emailError ? 'is-invalid' : null}" bind:value={ctx.email} placeholder={$_("summary.email")} name="email" required>
							{#if ctx.emailError}
								<div class="invalid-feedback">{$_("summary.validationErrors.email")}</div>
							{/if}
						</div>
						<div class="col-12 col-md-4 input-group-lg">
							<input type="text" class="form-control mx-1 shadow my-1 {ctx.phoneError ? 'is-invalid' : null}" bind:value={ctx.phone} placeholder={$_("summary.phone")} name="phone" required>
							{#if ctx.phoneError}
								<div class="invalid-feedback">{$_("summary.validationErrors.phone")}</div>
							{/if}
						</div>
					</div>
					{#if  $basketData.deliveryMode == 0}
						<h3 class="pt-3">{$_("summary.delivery") }</h3>
						<div class="row my-2">
							<div class="col-12 col-md-4 input-group-lg">
								<input type="text" class="form-control mx-1 shadow my-1" value={$selectedAddress.city} placeholder={$_("summary.deliveryAddr.city")} name="city" required readonly>
							</div>
							<div class="col-12 col-md-4 input-group-lg">
								<input type="text" class="form-control mx-1 shadow my-1" value={$selectedAddress.street} placeholder={$_("summary.deliveryAddr.street")} name="street" required readonly>
							</div>
							<div class="col-6 col-md-2 input-group-lg">
								<input type="text" class="form-control mx-1 shadow my-1" value={$selectedAddress.streetNumber} placeholder={$_("summary.deliveryAddr.streetNumber")} name="home" required readonly>
							</div>
							<div class="col-6 col-md-2 input-group-lg">
								<input type="text" class="form-control mx-1 shadow my-1" value={$selectedAddress.houseNumber} placeholder={$_("summary.deliveryAddr.houseNumber")} name="apartment" required>
							</div>
							<div class="col-12 col-md-12 input-group-lg">
								<input type="text" class="form-control mx-1 shadow my-1 mt-3"  bind:value={ctx.note} placeholder={$_("summary.deliveryAddr.note")} name="note">
							</div>
						</div>
					{/if}
					{#if  $basketData.deliveryMode == 1}
						<h3 class="pt-3">{$_("summary.pickupByPerson") }</h3>
						<div class="row mx-1 my-2">
							<h4>Restauracja {$selectedRestaurant.name}</h4>
							<p>{$selectedRestaurant.city}, {$selectedRestaurant.address}</p>
						</div>
					{/if}
				</div>
				<div class="row py-1 mx-1 my-2 border border-1 bg-white" style="border-color: darkgrey">
					<h3 class="pt-3">{$_("summary.payment") }</h3>
					<div class="row">
						<div class="col-sm-6">
						<div class="card">
							<div class="card-body">
							<h5 class="card-title">{$_("summary.discountCode") }</h5>
							<input type="text" name="code" bind:value={code} class="form-control shadow border border-1 my-3" style="border-color: darkgrey">
							<button class="btn btn-outline-success shadow" on:click={applyDiscountCode}>{$_("summary.applyDiscountCode")}</button>
							{#if showInvalidCodeAlert}
								<div class="alert alert-danger" role="alert">
									{$_("summary.invalidDiscountCode") }
								</div>
							{/if}
							
							{#if $basketData.discountLoaded }
								<button class="btn btn-outline-danger shadow" on:click={removeDiscountCode}>{$_("summary.removeDiscountCode")}</button>
							{/if}
							</div>
						</div>
						</div>
						<div class="col-sm-6">
						<div class="card">
							<div class="card-body">
								<h5 class="card-title">{$_("summary.documentType") }</h5>
								<div class="form-check form-check-inline">
									<input class="form-check-input" type="radio" id="paragon"  bind:group={ctx.invoice}  value={false} name="document" checked>
									<label class="form-check-label" for="paragon" onclick="invoice('no')">{$_("summary.fiscalDocument") }</label>
								</div>
								<div class="form-check form-check-inline">
									<input class="form-check-input" type="radio" id="invoice" name="document"  bind:group={ctx.invoice}  value={true} >
									<label class="form-check-label" for="invoice" >{$_("summary.invoice") }</label>
								</div>
								{#if ctx.invoice}
									<div>
										<h5>{$_("summary.dataForInvoice") }</h5>
										<input type="text" bind:value={ctx.invoiceNip} class="form-control shadow border border-1 my-3  {ctx.invoiceNipError ? 'is-invalid' : null}" placeholder="NIP">
										{#if ctx.invoiceNipError}
											<div class="payment-invalid">{$_("summary.validationErrors.required")}</div>
										{/if}		
										<input type="text" bind:value={ctx.invoiceName} class="form-control shadow border border-1 my-3  {ctx.invoiceNameError ? 'is-invalid' : null}" placeholder={$_('summary.invoiceFirmName')}>
										{#if ctx.invoiceNameError}
											<div class="payment-invalid">{$_("summary.validationErrors.required")}</div>
										{/if}		
										<input type="text" bind:value={ctx.invoiceAddr} class="form-control shadow border border-1 my-3  {ctx.invoiceAddrError ? 'is-invalid' : null}" placeholder={$_('summary.invoiceAddr')}>
										{#if ctx.invoiceAddrError}
											<div class="payment-invalid">{$_("summary.validationErrors.required")}</div>
										{/if}									
									</div>
								{/if}
								<h5 class="card-title">{$_("summary.paymentType") }</h5>
								<div class="btn-group" role="group" aria-label="Basic radio toggle button group">
									{#each payments as payment }
										<input type="radio" bind:group={ctx.paymentTypeId}  class="btn-check" value={payment.id} name="paymentTypeId" id={payment.id} autocomplete="off" required>
										<label class="btn btn-outline-success {ctx.paymentTypeIdError ? 'payment-is-invalid' : null}" for={payment.id}>{payment.name}</label>
									{/each}
								</div>
								
								{#if ctx.paymentTypeIdError}
									<div class="payment-invalid">{$_("summary.validationErrors.paymentError")}</div>
								{/if}
							</div>
						</div>
						</div>
					</div>
				</div>
				<div class="card">
					<div class="card-body">
						<div class="form-check">
							<input class="form-check-input" type="checkbox"  name="regulamin"  bind:checked={ctx.requiredTerms}  id="regulamin" required>
							<label class="form-check-label {ctx.requiredTermsError ? 'terms-invalid' : null}" for="regulamin">
								<Terms/>
							</label>
						</div>
						{#if ctx.requiredTermsError}
							<div class="payment-invalid">{$_("summary.validationErrors.requiredTermsError")}</div>
						{/if}
						{#each terms as term }
							<div class="form-check">
								<input class="form-check-input" type="checkbox"  bind:group={ctx.termSelection} value={term.id} id={'term' + term.id} name="regulamin2">
								<label class="form-check-label" for={'term' +term.id}>
									{ term.content }
								</label>
							</div>
						{/each}
						
						<div class="button-group justify-content-end mt-3" role="group" >
							<button type="button"  on:click|preventDefault|stopPropagation={goBack} class="btn btn-outline-secondary btn-lg mx-2 my-2 px-5 py-2">{$_("summary.goBack") }</button>
							<button type="button" disabled={processing}  on:click|preventDefault|stopPropagation={goCreateOrder} class="btn btn-outline-danger btn-lg mx-2 my-2 px-5 py-2 fw-bold">{$_("summary.createOrder") }</button>
						</div>
					</div>
				</div>
			{/if}
	
		</div>
		<div class="col-12 col-sm-4 col-xxl-4 col-xl-4 order-xxl-2 order-xl-2 order-1 justify-content-center align-items-center px-xxl-5 px-xl-4">
			<RestaurantInfo />
			<Basket onSummary={true} />
		</div>
	</div>
</CommonLayout>
<style>
	.terms-invalid {
		color: red;
	}
	.payment-invalid {
		color: red;
	}
	.payment-is-invalid {
		border-color: red !important;
	}
	.sending-box {
		height: 500px;
	}
</style>
