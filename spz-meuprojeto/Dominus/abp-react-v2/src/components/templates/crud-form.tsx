import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Loader2 } from "lucide-react";

// This is a generic CRUD form template that will be used by the generator
// Example for a "Workspace" entity

const formSchema = z.object({
    name: z.string().min(2, "Name must be at least 2 characters"),
    slug: z.string().min(2, "Slug must be at least 2 characters").regex(/^[a-z0-9-]+$/, "Lowecase, numbers and dashes only"),
    adminEmail: z.string().email("Invalid email address"),
    adminPassword: z.string().min(6, "Password must be at least 6 characters").optional(),
});

type FormValues = z.infer<typeof formSchema>;

interface CrudFormProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (values: FormValues) => void;
    initialValues?: Partial<FormValues>;
    title: string;
    description?: string;
    isLoading?: boolean;
}

export function CrudForm({
    isOpen,
    onClose,
    onSubmit,
    initialValues,
    title,
    description,
    isLoading,
}: CrudFormProps) {
    const isEditing = !!initialValues;

    const {
        register,
        handleSubmit,
        formState: { errors },
        reset,
    } = useForm<FormValues>({
        resolver: zodResolver(formSchema),
        defaultValues: initialValues || {
            name: "",
            slug: "",
            adminEmail: "",
        },
    });

    const onFormSubmit = (data: FormValues) => {
        onSubmit(data);
        if (!isLoading) {
            reset();
        }
    };

    return (
        <Dialog open={isOpen} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>{title}</DialogTitle>
                    {description && <DialogDescription>{description}</DialogDescription>}
                </DialogHeader>
                <form onSubmit={handleSubmit(onFormSubmit)} className="space-y-4 py-4">
                    <div className="space-y-2">
                        <Label htmlFor="name">Name</Label>
                        <Input id="name" {...register("name")} placeholder="My Awesome Workspace" />
                        {errors.name && <p className="text-xs text-destructive">{errors.name.message}</p>}
                    </div>

                    <div className="space-y-2">
                        <Label htmlFor="slug">Slug</Label>
                        <Input id="slug" {...register("slug")} placeholder="my-awesome-workspace" />
                        {errors.slug && <p className="text-xs text-destructive">{errors.slug.message}</p>}
                    </div>

                    <div className="space-y-2">
                        <Label htmlFor="adminEmail">Admin Email</Label>
                        <Input id="adminEmail" type="email" {...register("adminEmail")} placeholder="admin@example.com" />
                        {errors.adminEmail && <p className="text-xs text-destructive">{errors.adminEmail.message}</p>}
                    </div>

                    {!isEditing && (
                        <div className="space-y-2">
                            <Label htmlFor="adminPassword">Admin Password</Label>
                            <Input id="adminPassword" type="password" {...register("adminPassword")} placeholder="••••••••" />
                            {errors.adminPassword && <p className="text-xs text-destructive">{errors.adminPassword.message}</p>}
                        </div>
                    )}

                    <DialogFooter>
                        <Button type="button" variant="outline" onClick={onClose} disabled={isLoading}>
                            Cancel
                        </Button>
                        <Button type="submit" disabled={isLoading}>
                            {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                            {isEditing ? "Save Changes" : "Create"}
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
}
