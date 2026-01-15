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
  import { useCreateClientContact, useUpdateClientContact } from "@/lib/abp/hooks/useClientContacts";
import { toast } from "sonner";

const formSchema = z.object({
  
    name: z.any(),
  
    role: z.any(),
  
    email: z.any(),
  
    phone: z.any(),
  
    isPrimary: z.any(),
  
    clientId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface ClientContactFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function ClientContactForm({
  isOpen,
  onClose,
  initialValues,
}: ClientContactFormProps) {
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

  const createMutation = useCreateClientContact();
const updateMutation = useUpdateClientContact();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("ClientContact updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("ClientContact created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save clientcontact:", error);
    toast.error(error.message || "Failed to save clientcontact");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit ClientContact": "Create ClientContact" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the clientcontact." : "Fill in the details to create a new clientcontact." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="name" > Name * </Label>

<Input id="name" {...register("name") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="role" > Role</Label>

<Input id="role" {...register("role") } />

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
  <Label htmlFor="isPrimary" > IsPrimary</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="isPrimary"
checked = { watch("isPrimary") }
onCheckedChange = {(checked) => setValue("isPrimary", !!checked)}
              />
  < label htmlFor = "isPrimary" className = "text-sm font-normal" >
    { watch("isPrimary") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="clientId" > ClientId</Label>

<Input id="clientId" {...register("clientId") } />

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
