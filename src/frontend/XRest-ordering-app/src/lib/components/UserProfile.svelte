<script>
	import { _ } from "../services/localization/i18n";
	import {
		logoff,
		authData,
		setCallbackToFireWhenUnauthorized,
		setRefrestTokenCallback
	} from '../services/customer'
	import { onMount } from 'svelte'
	import { push } from '../router'

	setCallbackToFireWhenUnauthorized(()=> {
		push('/')
	})

	async function register() {
		push('/customer/register')
	}

	async function logon() {
		push('/customer/logon')
	}

	
	async function callLogoff() {
		logoff()
		push('/')
	}
	
	async function callSite(site) {
		push(`/customer/profile/${site}`)
	}

	onMount(async () =>{
		setRefrestTokenCallback() 
	})

</script>

<div class="mx-auto links">
	{#if $authData.isLogged}
		<div class="col-md-3 text-start dropdown">
			<button type="button" class="btn btn-light text-uppercase dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false"><i class="fas fa-user"></i>{$_("userProfile.hello")} {$authData.user}</button>
			<ul class="dropdown-menu">
				<!-- svelte-ignore a11y-invalid-attribute -->
				<li><a class="dropdown-item" href="#" on:click|preventDefault|stopPropagation={() => callSite('data')}>{$_("userProfile.yourProfile")}</a></li>
				<!-- svelte-ignore a11y-invalid-attribute -->
				<li><a class="dropdown-item" href="#" on:click|preventDefault|stopPropagation={() => callSite('favorite-addresses')}>{$_("userProfile.favAddr")}</a></li>
				<!-- svelte-ignore a11y-invalid-attribute -->
				<li><a class="dropdown-item" href="#" on:click|preventDefault|stopPropagation={() => callSite('order-history')}>{$_("userProfile.orderHistory")}</a></li>
				<!-- svelte-ignore a11y-invalid-attribute -->
				<li><a class="dropdown-item" href="#" on:click|preventDefault|stopPropagation={() => callSite('points')}>{$_("userProfile.points.title")}</a></li>
				<li><hr class="dropdown-divider"></li>
				<!-- svelte-ignore a11y-invalid-attribute -->
				<li><a class="dropdown-item" href="#" on:click|preventDefault|stopPropagation={callLogoff}>Wyloguj się</a></li>
				</ul>
		</div>
	{:else}
		<button
			type="button"
			on:click|preventDefault|stopPropagation={logon}
			class="btn btn-light text-uppercase"><i class="fas fa-user" /> {$_("userProfile.logon")}</button> /
		<button
			type="button"
			on:click|preventDefault|stopPropagation={register}
			class="btn btn-light text-uppercase"><i class="fas fa-user" /> {$_("userProfile.register")}</button>
	{/if}
</div>

<style>
	.links {
		color: white;
	}

	@media only screen and (max-width: 700px) {
		.links {
			background-color: #292c31;
			width: 100%;
			text-align: center;
			padding: 0.3rem !important;
			margin-top: 5px;
		}
	}
</style>
