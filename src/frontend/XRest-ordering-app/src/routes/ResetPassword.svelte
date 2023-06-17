<script>
    import { _ } from '../lib/services/localization/i18n'
    import LayoutSimple from './_LayoutSimple.svelte'
    import WaitingButton from '../lib/components/WatingButton.svelte'
    import { resetPassword } from '../lib/services/customer'
    import { isValidPassword } from '../lib/helpers'
    import { routeParams } from '../lib/router' 

    async function validateAndResetPassword() {
        showCriticalError = false
        showOk = false
        if(!isValidPassword(ctx.password, ctx.retypedPassword)) {
            showError($_('register.errors.password'))
            return 
        }

        sending = true
        let result = await resetPassword(ctx)
        sending = false
        switch(result.status){
            case 'ok':
                showOk = true
                break;
            default:
                showError($_('logon.resetPasswordError'))
                break;
        }
    }
    
    function showError(text) {
		showCriticalError = true
		criticalErrorMessage = text
	}

    let ctx = {
        password: '',
        retypedPassword: '',
        token:  $routeParams.encodedToken
    }

    let showCriticalError = false
    let criticalErrorMessage = ''
    let showOk = false
    let sending = false

    async function goStart() {
        push('/')
    }
</script>
<LayoutSimple>
    <div class="container-fluid">
        <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
            <div class="col-12">
                <img
                    src="assets/img/logo.png"
                    class="mx-auto d-block img-fluid my-5"
                    alt="logo"
                />
            </div>
        </div>
        <div class="mx-auto card my-5 col-md-11 col-xxl-7">
            <div class="card-header bg-white">
                <ul class="nav card-header-pills justify-content-center fs-4 search-address">
                    <li class="nav-item text-uppercase">{$_('logon.resetPasswordTitle')}</li>
                </ul>
            </div>
            <div class="card-body ">
                {#if showCriticalError}
                    <div class="alert alert-danger" role="alert">
                        {criticalErrorMessage}
                    </div>
                {/if}
                {#if showOk}
                    <div class="text-center">
                        <div class="alert alert-success " role="alert">
                            {$_('logon.resetPasswordOk')}
                        </div>
                        <button
                            type="input"
                            on:click|preventDefault|stopPropagation={goStart}
                            class="btn kk-color text-uppercase text-white shadow">
                            {$_('error.goStart')}
                        </button> 
                    </div>
                {/if}
                {#if !showOk}
                <div class="mb-3">
                    <label for="email" class="form-label">{$_('register.password')}</label>
                    <input type="password" bind:value={ctx.password} class="form-control shadow">
                </div>
                <div class="mb-3">
                    <label for="email" class="form-label">{$_('register.reenterPassword')}</label>
                    <input type="password" bind:value={ctx.retypedPassword} class="form-control shadow">
                </div>     
                <div class="mb-3 justify-content-end text-end">
                    <WaitingButton callback={validateAndResetPassword}>
                        {$_('logon.resetPassword')}
                    </WaitingButton>
                </div>
                {/if}
            </div>
        </div>
    </div>
</LayoutSimple>
<style>
.text-center {
    text-align:  center;
}
</style>