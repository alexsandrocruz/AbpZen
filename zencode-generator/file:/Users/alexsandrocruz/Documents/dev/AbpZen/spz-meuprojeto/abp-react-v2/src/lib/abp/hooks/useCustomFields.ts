import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetCustomFieldsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useCustomFields(input: GetCustomFieldsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["customFields", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/custom-field", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllCustomFields() {
  return useQuery({
    queryKey: ["customFields", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/custom-field", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useCustomField(id: string) {
  return useQuery({
    queryKey: ["customField", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/custom-field/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateCustomField() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/custom-field", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["customFields"] });
    },
  });
}

export function useUpdateCustomField() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/custom-field/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["customFields"] });
      queryClient.invalidateQueries({ queryKey: ["customField", data.id] });
    },
  });
}

export function useDeleteCustomField() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/custom-field/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["customFields"] });
    },
  });
}


