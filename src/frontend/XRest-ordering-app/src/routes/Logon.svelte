<script>
    import LayoutSimple from './_LayoutSimple.svelte'
    import WaitingButton from '../lib/components/WatingButton.svelte'
    import { _ } from '../lib/services/localization/i18n' 
    import { isEmpty } from '../lib/helpers'
    import { logon } from '../lib/services/customer'
    import { selectedRestaurant } from '../lib/stores'
    import { basketData } from '../lib/services/basketApi'
    import { push } from '../lib/router'
     
    async function remindPassword() {
        push('/customer/send-reset-password')
    }

    function makeLogon() {
        showCriticalError = false
        if (isEmpty(ctx.email) || isEmpty(ctx.password)) {
            showError($_('logon.enterData')) 
            return
        }

        logon(ctx, (status) => {
            if (status == 'ok') {
                if ($selectedRestaurant.id != 0) {
                    const alias = $selectedRestaurant.alias
                    const transportZoneId = $selectedRestaurant.transportZoneID
                    const deliveryMode = $basketData.deliveryMode
                    push(`/restaurant/${alias}/${transportZoneId}/${deliveryMode}`)
                } else {
                    push('/')
                }
            } else {
                showError($_('logon.showError'))
            }  
        })
    }

    function showError(text) {
		showCriticalError = true
		criticalErrorMessage = text
	}

    let ctx = {
        email: '',
        password: ''
    }
    
    let showCriticalError = false
    let criticalErrorMessage = ''

    let makeLogonProcessing = false
    function makeLogonProcessingHandle(newValue) {
        if (newValue) {
            showCriticalError = false
        }
        
        makeLogonProcessing = newValue
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
        
        <div class="mx-auto card my-5 col-md-11 col-xxl-7">
            <div class="card-header bg-white">
                <ul class="nav card-header-pills justify-content-center fs-4 search-address">
                    <li class="nav-item text-uppercase">{$_('logon.title')}</li>
                </ul>
            </div>
            <div class="card-body">
                {#if showCriticalError}
                    <div class="alert alert-danger" role="alert">
                        {criticalErrorMessage}
                    </div>
                {/if}
                <div class="mb-3">
                    <label for="email" class="form-label">{$_('logon.email')}</label>
                    <input disabled={makeLogonProcessing} type="email" class="form-control shadow" bind:value={ctx.email} placeholder="">
                </div>
                <div class="mb-3">
                    <label for="pass" class="form-label">{$_('logon.password')}</label>
                    <input disabled={makeLogonProcessing} type="password" class="form-control shadow"  bind:value={ctx.password}>
                </div>
                <div class="mb-3 justify-content-end text-end">
                    <button class="btn btn-outline-success shadow" on:click|preventDefault|stopPropagation={remindPassword}>
                        {$_('logon.forgetPassword')}
                    </button>
                    <WaitingButton callback={makeLogon} processingCallback={makeLogonProcessingHandle}>
                        {$_('logon.logon')}
                    </WaitingButton>
                </div>
            </div>
        </div>
    </div>
</LayoutSimple>
