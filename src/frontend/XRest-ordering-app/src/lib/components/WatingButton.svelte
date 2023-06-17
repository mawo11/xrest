<script>
    export let processing = false
    export let processingCallback = null
    export let callback = null

    async function fireCallback(e) {
        processing = true;
        fireProcessingCallback(processing)
        setTimeout(() => {
            if (callback != null) {
                callback(e);
            }
            processing = false;
            fireProcessingCallback(processing)
        }, 1000);
    }

    function fireProcessingCallback(value) {
        try {
            if (processingCallback != null) {
                processingCallback(value);
            }
        }catch(e){

        }
    }
</script>
<button
    class="btn btn-outline-success shadow"
    type="button"
    disabled={processing}
    on:click|preventDefault|stopPropagation={fireCallback}>
    {#if processing}
        <span class="spinner-grow spinner-grow-sm" role="status" aria-hidden="true" />
    {/if}
    <slot />
</button>
