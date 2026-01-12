import { Toaster as Sonner } from "sonner";

export function Toaster() {
    return (
        <Sonner
            position="top-right"
            toastOptions={{
                classNames: {
                    toast: "bg-background text-foreground border-border",
                    description: "text-muted-foreground",
                    actionButton: "bg-primary text-primary-foreground",
                    cancelButton: "bg-muted text-muted-foreground",
                },
            }}
        />
    );
}
