import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLegalProcessesInput {
  filter?: string;
  skipCount?: number;
  maxResultCount?: number;
}

export function useLegalProcesses(input: GetLegalProcessesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10 } = input;

  return useQuery({
    queryKey: ["legalProcesses", filter, skipCount, maxResultCount],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/legal-process", {
        params: {
          filter,
          skipCount,
          maxResultCount,
        },
      });
      return response.data;
    },
  });
}

export function useLegalProcess(id: string) {
  return useQuery({
    queryKey: ["legalProcess", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/legal-process/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLegalProcess() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/legal-process", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["legalProcesses"] });
    },
  });
}

export function useUpdateLegalProcess() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/legal-process/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["legalProcesses"] });
      queryClient.invalidateQueries({ queryKey: ["legalProcess", data.id] });
    },
  });
}

export function useDeleteLegalProcess() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/legal-process/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["legalProcesses"] });
    },
  });
}
