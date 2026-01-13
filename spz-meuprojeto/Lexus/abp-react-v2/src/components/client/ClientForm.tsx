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
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateClient, useUpdateClient } from "@/lib/abp/hooks/useClients";
import { toast } from "sonner";

const formSchema = z.object({
  
    name: z.any(),
  
    email: z.any(),
  
    phone: z.any(),
  
    cpfCnpj: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface ClientFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function ClientForm({
  isOpen,
  onClose,
  initialValues,
}: ClientFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateClient();
const updateMutation = useUpdateClient();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Client updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Client created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save client:", error);
    toast.error(error.message || "Failed to save client");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Client": "Create Client" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the client." : "Fill in the details to create a new client." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="name" > Name * </Label>

<Input id="name" {...register("name") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="email" > Email * </Label>

<Input id="email" {...register("email") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="phone" > Phone</Label>

<Input id="phone" {...register("phone") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cpfCnpj" > CpfCnpj * </Label>

<Input id="cpfCnpj" {...register("cpfCnpj") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
