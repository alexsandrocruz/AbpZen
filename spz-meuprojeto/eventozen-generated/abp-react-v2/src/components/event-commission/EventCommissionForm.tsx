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
  import { useCreateEventCommission, useUpdateEventCommission } from "@/lib/abp/hooks/useEventCommissions";
import { toast } from "sonner";

const formSchema = z.object({
  
    description: z.any(),
  
    value: z.any(),
  
    percentage: z.any(),
  
    eventId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface EventCommissionFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function EventCommissionForm({
  isOpen,
  onClose,
  initialValues,
}: EventCommissionFormProps) {
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

  const createMutation = useCreateEventCommission();
const updateMutation = useUpdateEventCommission();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("EventCommission updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("EventCommission created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save eventcommission:", error);
    toast.error(error.message || "Failed to save eventcommission");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit EventCommission": "Create EventCommission" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the eventcommission." : "Fill in the details to create a new eventcommission." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="description" > Description</Label>

<Input id="description" {...register("description") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="value" > Value</Label>

<Input id="value" type = "number" step = "any" {...register("value") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="percentage" > Percentage</Label>

<Input id="percentage" type = "number" step = "any" {...register("percentage") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="eventId" > EventId</Label>

<Input id="eventId" {...register("eventId") } />

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
