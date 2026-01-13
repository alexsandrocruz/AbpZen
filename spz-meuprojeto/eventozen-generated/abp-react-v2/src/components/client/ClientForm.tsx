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
  
    type: z.any(),
  
    document: z.any(),
  
    email: z.any(),
  
    phone: z.any(),
  
    address: z.any(),
  
    city: z.any(),
  
    state: z.any(),
  
    notes: z.any(),
  
    isActive: z.any(),
  
    leadStatus: z.any(),
  
    firstContactDate: z.any(),
  
    lastContactDate: z.any(),
  
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
  <Label htmlFor="type" > Type * </Label>

<Input id="type" {...register("type") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="document" > Document</Label>

<Input id="document" {...register("document") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="email" > Email</Label>

<Input id="email" {...register("email") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="phone" > Phone</Label>

<Input id="phone" {...register("phone") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="address" > Address</Label>

<Input id="address" {...register("address") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="city" > City</Label>

<Input id="city" {...register("city") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="state" > State</Label>

<Input id="state" {...register("state") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="notes" > Notes</Label>

<Input id="notes" {...register("notes") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="isActive" > IsActive</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="isActive"
checked = { watch("isActive") }
onCheckedChange = {(checked) => setValue("isActive", !!checked)}
              />
  < label htmlFor = "isActive" className = "text-sm font-normal" >
    { watch("isActive") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="leadStatus" > LeadStatus</Label>

<Input id="leadStatus" {...register("leadStatus") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="firstContactDate" > FirstContactDate</Label>

<Input id="firstContactDate" type = "date" {...register("firstContactDate") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="lastContactDate" > LastContactDate</Label>

<Input id="lastContactDate" type = "date" {...register("lastContactDate") } />

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
