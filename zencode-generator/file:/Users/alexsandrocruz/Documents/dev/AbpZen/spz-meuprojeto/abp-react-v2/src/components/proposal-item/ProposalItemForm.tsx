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
  import { useCreateProposalItem, useUpdateProposalItem } from "@/lib/abp/hooks/useProposalItems";
import { toast } from "sonner";

const formSchema = z.object({
  
    description: z.any(),
  
    quantity: z.any(),
  
    price: z.any(),
  
    proposalId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface ProposalItemFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function ProposalItemForm({
  isOpen,
  onClose,
  initialValues,
}: ProposalItemFormProps) {
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

  const createMutation = useCreateProposalItem();
const updateMutation = useUpdateProposalItem();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("ProposalItem updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("ProposalItem created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save proposalitem:", error);
    toast.error(error.message || "Failed to save proposalitem");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit ProposalItem": "Create ProposalItem" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the proposalitem." : "Fill in the details to create a new proposalitem." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="description" > Description * </Label>

<Input id="description" {...register("description") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="quantity" > Quantity * </Label>

<Input id="quantity" type = "number" step = "any" {...register("quantity") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="price" > Price * </Label>

<Input id="price" type = "number" step = "any" {...register("price") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="proposalId" > ProposalId</Label>

<Input id="proposalId" {...register("proposalId") } />

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
