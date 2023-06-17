<script>
    import { _ } from '../lib/services/localization/i18n'
    import LayoutSimple from './_LayoutSimple.svelte'
    import WaitingButton from '../lib/components/WatingButton.svelte'
    import { sendResetPassword } from '../lib/services/customer'
    import { isValidEmail } from '../lib/helpers'

    async function resetPassword() {
        showCriticalError = false
        showOk = false
        if (!isValidEmail(ctx.email)){
            showError($_('register.errors.email'))
            return 
        }

        let result = await sendResetPassword(ctx)
        switch(result.status){
            case 'ok':
                showOk = true
                break;
            default:
                showError($_('logon.sendPasswordError'))
                break;
        }
    }

    function showError(text) {
		showCriticalError = true
		criticalErrorMessage = text
	}

    let ctx = {
        email: ''
    }

    let criticalErrorMessage = ''
    let showCriticalError = false
    let showOk = false
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
                    <li class="nav-item text-uppercase">{$_('logon.sendResetPassword')}</li>
                </ul>
            </div>
            <div class="card-body">
                {#if showCriticalError}
                    <div class="alert alert-danger" role="alert">
                        {criticalErrorMessage}
                    </div>
                {/if}
                {#if showOk}
                    <div class="alert alert-success" role="alert">
                        {$_('logon.sendPasswordOk')}
                    </div>
                {/if}
                <div class="mb-3">
                    <label for="email" class="form-label">{$_('logon.email')}</label>
                    <input type="email" class="form-control shadow" bind:value={ctx.email} placeholder="">
                </div>               
                <div class="mb-3 justify-content-end text-end">
                    <WaitingButton callback={resetPassword}>
                        {$_('logon.sendResetPassword')}
                    </WaitingButton>
                </div>
            </div>
        </div>
    </div>
</LayoutSimple>