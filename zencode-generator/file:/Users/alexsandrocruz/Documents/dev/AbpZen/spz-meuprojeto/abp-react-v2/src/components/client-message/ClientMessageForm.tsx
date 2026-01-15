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
  import { useCreateClientMessage, useUpdateClientMessage } from "@/lib/abp/hooks/useClientMessages";
import { toast } from "sonner";

const formSchema = z.object({
  
    channel: z.any(),
  
    direction: z.any(),
  
    subject: z.any(),
  
    content: z.any(),
  
    sentAt: z.any(),
  
    clientId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface ClientMessageFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function ClientMessageForm({
  isOpen,
  onClose,
  initialValues,
}: ClientMessageFormProps) {
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

  const createMutation = useCreateClientMessage();
const updateMutation = useUpdateClientMessage();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("ClientMessage updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("ClientMessage created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save clientmessage:", error);
    toast.error(error.message || "Failed to save clientmessage");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit ClientMessage": "Create ClientMessage" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the clientmessage." : "Fill in the details to create a new clientmessage." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="channel" > Channel * </Label>

<Input id="channel" {...register("channel") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="direction" > Direction * </Label>

<Input id="direction" {...register("direction") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="subject" > Subject</Label>

<Input id="subject" {...register("subject") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="content" > Content * </Label>

<Input id="content" {...register("content") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="sentAt" > SentAt</Label>

<Input id="sentAt" type = "date" {...register("sentAt") } />

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
