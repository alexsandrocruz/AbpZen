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
  import { useCreateLeadWorkflow, useUpdateLeadWorkflow } from "@/lib/abp/hooks/useLeadWorkflows";
import { toast } from "sonner";

const formSchema = z.object({
  
    name: z.any(),
  
    isActive: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface LeadWorkflowFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function LeadWorkflowForm({
  isOpen,
  onClose,
  initialValues,
}: LeadWorkflowFormProps) {
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

  const createMutation = useCreateLeadWorkflow();
const updateMutation = useUpdateLeadWorkflow();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("LeadWorkflow updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("LeadWorkflow created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save leadworkflow:", error);
    toast.error(error.message || "Failed to save leadworkflow");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit LeadWorkflow": "Create LeadWorkflow" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the leadworkflow." : "Fill in the details to create a new leadworkflow." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="name" > Name * </Label>

<Input id="name" {...register("name") } />

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
