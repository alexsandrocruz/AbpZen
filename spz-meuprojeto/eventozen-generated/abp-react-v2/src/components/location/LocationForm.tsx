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
  import { useCreateLocation, useUpdateLocation } from "@/lib/abp/hooks/useLocations";
import { toast } from "sonner";

const formSchema = z.object({
  
    name: z.any(),
  
    address: z.any(),
  
    city: z.any(),
  
    state: z.any(),
  
    capacity: z.any(),
  
    zipCode: z.any(),
  
    notes: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface LocationFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function LocationForm({
  isOpen,
  onClose,
  initialValues,
}: LocationFormProps) {
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

  const createMutation = useCreateLocation();
const updateMutation = useUpdateLocation();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Location updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Location created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save location:", error);
    toast.error(error.message || "Failed to save location");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Location": "Create Location" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the location." : "Fill in the details to create a new location." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="name" > Name * </Label>

<Input id="name" {...register("name") } />

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
  <Label htmlFor="capacity" > Capacity</Label>

<Input id="capacity" type = "number" step = "any" {...register("capacity") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="zipCode" > ZipCode</Label>

<Input id="zipCode" {...register("zipCode") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="notes" > Notes</Label>

<Input id="notes" {...register("notes") } />

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
