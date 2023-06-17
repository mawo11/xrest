<script>
    import LayoutSimple from './_LayoutSimple.svelte'
    import Terms from '../lib/components/Terms.svelte'
    import WaitingButton from '../lib/components/WatingButton.svelte'
    import { _ } from '../lib/services/localization/i18n'
    import { 
        isValidEmail,
        isValidPassword
    } from '../lib/helpers'
    import { newAccount } from '../lib/services/customer'

    async function createNewAccount() {
        showCriticalError = false
        if (!isValidEmail(ctx.email)){
            showError($_('register.errors.email'))
            return 
        }
        
        if(!isValidPassword(ctx.password, ctx.retypedPassword)) {
            showError($_('register.errors.password'))
            return 
        }

        if (!ctx.terms) {
            showError($_('register.errors.terms'))
            return
        }

        let result = await newAccount(ctx)
        switch (result.status) {
            case 'ok':
                showOk = true
                break;
            case 'emailExists':
                showError($_('register.errors.emailExists'))
                break;
            case 'error':
                showError($_('register.errors.unknownError'))
                break;
        }
    }

    function showError(text) {
		showCriticalError = true
		criticalErrorMessage = text
	}

    let ctx = {
        email: '',
        password: '',
        retypedPassword: '',
        terms: false
    }
    
    let showCriticalError = false
    let criticalErrorMessage = ''
    let showOk = false

    let createNewAccountProcessing = false
    function createNewAccountProcessingHandle(newValue) {
        createNewAccountProcessing = newValue
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
                    <li class="nav-item text-uppercase">
                        {$_('register.title')}
                        </li>
                </ul>
            </div>
            <div class="card-body">
                {#if showCriticalError}
                    <div class="alert alert-danger" role="alert">
                        {criticalErrorMessage}
                    </div>
                {/if}
                {#if showOk}
                    <div class="alert alert-success text-center" role="alert">
                        {$_('register.registerOk')}
                    </div>
                {:else}
                    <div class="mb-3">
                        <label for="email" class="form-label">{$_('register.email')}</label>
                        <input type="text" disabled={createNewAccountProcessing} bind:value={ctx.email} class="form-control shadow">
                    </div>
                    <div class="mb-3">
                        <label for="email" class="form-label">{$_('register.password')}</label>
                        <input type="password" disabled={createNewAccountProcessing} bind:value={ctx.password} class="form-control shadow">
                    </div>
                    <div class="mb-3">
                        <label for="email" class="form-label">{$_('register.reenterPassword')}</label>
                        <input type="password" disabled={createNewAccountProcessing} bind:value={ctx.retypedPassword} class="form-control shadow">
                    </div>
                    <div class="mb-3">
                        <input class="form-check-input" disabled={createNewAccountProcessing} type="checkbox"  name="regulamin"  bind:checked={ctx.terms} id="regulamin" required>
                        <label class="form-check-label {ctx.requiredTermsError ? 'terms-invalid' : null}" for="regulamin">
                            <Terms/>
                        </label>
                    </div>
                    <div class="mb-3 justify-content-end text-end">
                        <WaitingButton callback={createNewAccount} processingCallback={createNewAccountProcessingHandle}>
                            {$_('register.register')}
                        </WaitingButton>
                    </div>
                {/if}
            </div>
        </div>
    </div>
</LayoutSimple>
